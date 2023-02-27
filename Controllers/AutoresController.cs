
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entities;

/**
Controller para manejar Autores.
*/
namespace WebApiAutores.Controllers
{
    [ApiController] // Decorator para indicar que esta clase es un ApiController
    [Route("api/autores")] // Aqui se declara la ruta del controlador
    public class AutoresController: ControllerBase  // Se debe derivar de la clase ControllerBase
    {
        private readonly ApplicationDbContext context;

        // Para enviar mensajes al log, el logger se debe colocar como dependencia de la clase.
        // ILogger<AutoresController> declara el tipo de la clase donde se va utilizar para identificar
        // de dónde provienen los mensajes.
        public AutoresController(ApplicationDbContext context, ILogger<AutoresController> logger)
        {
            // Aqui se aplica la inyección de dependencias.
            // El ApplicationDbContext es creado en la clase Startup
            // y por medio del constructor se puede obtener aqui para usarlo luego
            // para acceder a la db.
            this.context = context;
        }

        [HttpGet]  // Decorator que indica que se trata de un método HTTP GET
        public async Task<ActionResult<List<Autor>>> Get()
        {
            // Aqui se retorna la lista de autores en la db leyendo de forma asíncrona.
            // return await context.Autores.Include(x => x.Libros).ToListAsync();
            return await context.Autores.ToListAsync();
        }

        /**
        La palabra clave async es para indicar que se va utilizar programación asíncrona.
        Para los métodos asíncronos es requisito devolver Task<ActionResult> o ValueTask
        */
        [HttpPost]
        public async Task<ActionResult> Post (Autor autor)
        {
            // Ejemplo de validación desde el controlador hacia la db:
            var existeAutor = await context.Autores.AnyAsync(x => x.Nombre == autor.Nombre);

            if (existeAutor) {
                return BadRequest($"Ya existe un autor con el mismo nombre: {autor.Nombre}");
            }

            // Aqui se marca el Autor recibido como listo para guardarlo a la db, pero aún no se ha guardado
            context.Add(autor);

            // Aquí se guardan los cambios a la db de forma asíncrona
            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id:int}")] // ID del autor por medio de la ruta.
        public async Task<ActionResult> Put(Autor autor, int id)
        {
            // Aqui se consulta la tabla de autores para verificar si existe
            // algún autor con el ID recibido
            var existeAutor = await context.Autores.AnyAsync(x => x.Id == id);

            if (!existeAutor) 
            {
                return NotFound();
            }

            if (autor.Id != id) 
            {
                return BadRequest("El ID recibido no es igual al ID de la db");
            } 

            context.Update(autor);

            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            // Aqui se consulta la tabla de autores para verificar si existe
            // algún autor con el ID recibido
            var existeAutor = await context.Autores.AnyAsync(x => x.Id == id);

            if (!existeAutor) 
            {
                return NotFound();
            }

            // Aqui es necesario pasar un objeto de tipo Autor que representa
            // el elemento que va a ser eliminado.  Al menos debe tener el mismo
            // ID que el elemento a eliminar.
            context.Remove(new Autor() {Id = id});
            await context.SaveChangesAsync();

            return Ok();
        }

    }
}