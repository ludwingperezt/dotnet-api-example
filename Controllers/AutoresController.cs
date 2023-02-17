
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

        public AutoresController(ApplicationDbContext context)
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
            // Por ahora solo se retorna una lista creada en memoria.
            // return new List<Autor> () {
            //     new Autor() { Id=1, Nombre="Juan"},
            //     new Autor() { Id=2, Nombre="Homero"}
            // };

            // Aqui se retorna la lista de autores en la db leyendo de forma asíncrona.
            return await context.Autores.ToListAsync();
        }

        /**
        La palabra clave async es para indicar que se va utilizar programación asíncrona.
        Para los métodos asíncronos es requisito devolver Task<ActionResult> o ValueTask
        */
        [HttpPost]
        public async Task<ActionResult> Post (Autor autor)
        {
            // Aqui se marca el Autor recibido como listo para guardarlo a la db, pero aún no se ha guardado
            context.Add(autor);

            // Aquí se guardan los cambios a la db de forma asíncrona
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}