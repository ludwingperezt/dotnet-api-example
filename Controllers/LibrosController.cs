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
            // Aqui se obtiene la información de un libro y con la llamada a Include() se llama
            // también a la información del autor.
            // return await context.Libros.Include(x => x.Autor).FirstOrDefaultAsync(x => x.Id == id);

            // El .Include() hace un join para que en la misma consulta se obtengan los comentarios.
            // Para hacer Lazy Loading (recomendable) entonces lo mejor es omitir el Include() y quitar
            // el mapeo a la clase ComentarioDTO en la clase LibroDTO
            // var libro = await context.Libros.Include(libroDB => libroDB.Comentarios)
            //     .FirstOrDefaultAsync(libroDB => libroDB.Id == id);

            var libro = await context.Libros
                .FirstOrDefaultAsync(libroDB => libroDB.Id == id);

            return mapper.Map<LibroDTO>(libro);
        }

        [HttpPost]
        public async Task<ActionResult> Post(LibroCreacionDTO libroCreacion)
        {
            // Aqui se inserta un libro nuevo.
            // En primer lugar se verifica que exista el autor del libro en la DB
            // var existeAutor = await context.Autores.AnyAsync(x => x.Id == libro.AutorId);

            // if (!existeAutor)
            // {
            //     return BadRequest($"No existe el autor de Id:{libro.AutorId}");
            // }

            var libro = mapper.Map<Libro>(libroCreacion);

            context.Add(libro);
            await context.SaveChangesAsync();
            return Ok();
        }

    }
}