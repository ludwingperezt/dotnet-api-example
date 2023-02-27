using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entities;

namespace WebApiAutores
{
    // Esta es la clase central de Entity Framework Core.
    // Aquí se configuran las tablas de la base de datos.
    public class ApplicationDbContext: DbContext
    {
        /**
        A través de este constructor se pueden pasar cosas como el connection string.
        */
        public ApplicationDbContext(DbContextOptions options): base(options)
        {
            
        }

        // En esta línea lo que se indica a EF es que debe crear una tabla de nombre
        // "Autores" a partir del esquema definido en la clase Autor
        public DbSet<Autor> Autores { get; set; }

        // Se pone aqui el DbSet de libros para poder hacer queries directamente sobre
        // la tabla de libros sin tener que pasar antes por la tabla de Autores.
        public DbSet<Libro> Libros { get; set; }
    }
}