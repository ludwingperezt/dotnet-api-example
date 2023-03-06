using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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

        [HttpGet("{id:int}")]
        public async Task<ActionResult<LibroDTO>> Get(int id)
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

            return mapper.Map<LibroDTO>(libro);
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

            // Recorrer los autores para asignar el orden.
            if (libro.AutoresLibros != null)
            {
                for (int i=0; i < libro.AutoresLibros.Count; i++)
                {
                    libro.AutoresLibros[i].Orden = i;
                }
            }

            context.Add(libro);
            await context.SaveChangesAsync();
            return Ok();
        }

    }
}