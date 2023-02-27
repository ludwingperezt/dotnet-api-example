using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entities;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        public LibrosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // [HttpGet("{id:int}")]
        // public async Task<ActionResult<Libro>> Get(int id)
        // {
        //     // Aqui se obtiene la información de un libro y con la llamada a Include() se llama
        //     // también a la información del autor.
        //     return await context.Libros.Include(x => x.Autor).FirstOrDefaultAsync(x => x.Id == id);
        // }

        // [HttpPost]
        // public async Task<ActionResult> Post(Libro libro)
        // {
        //     // Aqui se inserta un libro nuevo.
        //     // En primer lugar se verifica que exista el autor del libro en la DB
        //     var existeAutor = await context.Autores.AnyAsync(x => x.Id == libro.AutorId);

        //     if (!existeAutor)
        //     {
        //         return BadRequest($"No existe el autor de Id:{libro.AutorId}");
        //     }

        //     context.Add(libro);
        //     await context.SaveChangesAsync();
        //     return Ok();
        // }

    }
}