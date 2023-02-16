
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet]  // Decorator que indica que se trata de un método HTTP GET
        public ActionResult<List<Autor>> Get()
        {
            // Por ahora solo se retorna una lista creada en memoria.
            return new List<Autor> () {
                new Autor() { Id=1, Nombre="Juan"},
                new Autor() { Id=2, Nombre="Homero"}
            };
        }
    }
}