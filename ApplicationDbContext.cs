using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entities;

namespace WebApiAutores
{
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


    }
}