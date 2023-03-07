using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entities;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public LibrosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}", Name ="obtenerLibro")]
        public async Task<ActionResult<LibroDTOConAutores>> Get(int id)
        {
            // El .Include() hace un join para que en la misma consulta se obtengan los comentarios.
            // Para hacer Lazy Loading (recomendable) entonces lo mejor es omitir el Include() y quitar
            // el mapeo a la clase ComentarioDTO en la clase LibroDTO
            // var libro = await context.Libros.Include(libroDB => libroDB.Comentarios)
            //     .FirstOrDefaultAsync(libroDB => libroDB.Id == id);

            var libro = await context.Libros
                .Include(libroDB => libroDB.AutoresLibros) // Aqui se incluye en la consulta, la relación muchos-a-muchos con autores
                .ThenInclude(autorLibroDB => autorLibroDB.Autor) // Aqui ya se incluye la información del Autor a través de la relación AutorLibro
                .FirstOrDefaultAsync(libroDB => libroDB.Id == id);

            // Ordenar los autores del libro según el orden asignado a cada uno.
            libro.AutoresLibros = libro.AutoresLibros.OrderBy(x => x.Orden).ToList();

            return mapper.Map<LibroDTOConAutores>(libro);
        }

        [HttpPost]
        public async Task<ActionResult> Post(LibroCreacionDTO libroCreacion)
        {
            if (libroCreacion.AutoresIds == null)
            {
                return BadRequest("No se puede crear libro sin autores");
            }

            // La siguiente sentencia LINQ se traduce a:
            // Ir a la tabla de autores y consultar los autores cuyo ID se encuente
            // en la lista de IDs que se está proporcionando (libroCreacion.AutoresIds)
            // y retornar solo los ID's de los autores encontrados.
            var autoresIds = await context.Autores
                .Where(autorDB => libroCreacion.AutoresIds.Contains(autorDB.Id))
                .Select(x => x.Id)
                .ToListAsync();

            // Si el conteo de autores encontrados no es el mismo que el de
            // autores recibidos en la petición, entonces uno de los errores
            // no fue encontrado.
            if (libroCreacion.AutoresIds.Count != autoresIds.Count)
            {
                return BadRequest("Uno de los autores no existe");
            }

            var libro = mapper.Map<Libro>(libroCreacion);
            AsignarOrdenAutores(libro);            

            context.Add(libro);
            await context.SaveChangesAsync();

            var libroCreadoDTO = mapper.Map<LibroDTO>(libro);

            return CreatedAtRoute("obtenerLibro", new { id = libro.Id }, libroCreadoDTO);
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, LibroCreacionDTO libroCreacionDto)
        {
            var libroDb = await context.Libros
                .Include(x => x.AutoresLibros)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (libroDb == null)
            {
                return NotFound();
            }

            // En esta sentencia lo que se hace es tomar los valores de las propiedades
            // del objeto libroCreacionDto y llevarlas a libroDb, manteniendo la
            // misma instancia, ya que esa instancia de tipo Libro está registrada
            // en la memoria de EF Core, por lo tanto cualquier cambio puede
            // ser persistido sin demasiada complejidad.
            libroDb = mapper.Map(libroCreacionDto, libroDb);
            
            // Reasignar el orden de autores luego de la actualización de campos
            AsignarOrdenAutores(libroDb);

            await context.SaveChangesAsync();

            return NoContent();
        }

        private void AsignarOrdenAutores(Libro libro)
        {
            // Recorrer los autores para asignar el orden.
            if (libro.AutoresLibros != null)
            {
                for (int i=0; i < libro.AutoresLibros.Count; i++)
                {
                    libro.AutoresLibros[i].Orden = i;
                }
            }
        }

        /**
        Con PATCH se pueden hacer actualizaciones parciales de una entidad.
        A diferencia de PUT, que lo que hace es sobreescribir aquellos datos
        que no se reciban en la petición HTTP, con PATCH se pueden actualizar solo
        los campos que se especifiquen.  Para ello se usa el estándar RFC 6902
        que corresponde a JSON Patch.  En ese estándar se especifica cómo hacer
        una actualización parcial. Un ejemplo de una petición de actualización
        parcial para cambiar solamente el título de un libro sería:
            [
                {
                    "path": "/titulo",
                    "op": "replace",
                    "value": "Nuevo título actualizado con patch"
                }
            ]

            Donde:
            - "path" especifica el campo a cambiar (debe llevar la / antes del nombre del campo)
            - "op" especifica qué operación se hará, en este caso es un reemplazo
                de la data original por la que se están enviando en la petición
                en el campo "value".
            - "value" Es el nuevo valor que se quere aplicar al campo.

        Para hacer todo este proceso es necesario instalar:
        dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson

        Y luego configurar .AddNewtonsoftJson() en la clase Startup, en el 
        método ConfigureServices().
        */
        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<LibroPatchDTO> patchDocument)
        {
            // si el Patch document es nulo significa que el documento enviado
            // por el cliente no es válido.
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var libroDb = await context.Libros.FirstOrDefaultAsync(x => x.Id == id);

            if (libroDb == null)
            {
                return NotFound();
            }

            // LO que se hace aquí es llenar el objeto de tipo LibroPatchDTO
            // con los datos que vienen de la base de datos (libroDb) y luego
            // a ese objeto se le aplican los cambios que se recibieron en el
            // patch document.            
            var libroDto = mapper.Map<LibroPatchDTO>(libroDb);
            // Si hay algún error entonces se coloca en el ModelState 
            patchDocument.ApplyTo(libroDto, ModelState);

            // Se hacen las validaciones de los campos.
            var esValido = TryValidateModel(libroDto);

            if (!esValido)
            {
                // En el ModelState van a estar todos los errores encontrados.
                return BadRequest(ModelState);
            }

            // Guardar los cambios.
            mapper.Map(libroDto, libroDb);
            await context.SaveChangesAsync();

            return NoContent();
        }

    }
}