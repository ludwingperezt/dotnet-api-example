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


        // Esta función se sobreescribe para poder configurar la creación de llaves 
        // primarias de la entidad AutorLibro.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // NO SE DEBE QUITAR la llamada a base.OnModelCreating()
            base.OnModelCreating(modelBuilder);

            // En este punto se configura la llave primaria combinada entre las llaves 
            // foráneas AutorId y LibroId en la entidad/tabla AutorLibro.
            // 
            modelBuilder.Entity<AutorLibro>() // Se indica que se agrega una configuración especial para la entidad AutorLibro
                .HasKey(autorLibro => new { autorLibro.AutorId, autorLibro.LibroId}); // Se indica que la llave primaria de la entidad AutorLibro es compuesta por las dos llaves foráneas
        } 

        // En esta línea lo que se indica a EF es que debe crear una tabla de nombre
        // "Autores" a partir del esquema definido en la clase Autor
        public DbSet<Autor> Autores { get; set; }

        // Se pone aqui el DbSet de libros para poder hacer queries directamente sobre
        // la tabla de libros sin tener que pasar antes por la tabla de Autores.
        public DbSet<Libro> Libros { get; set; }

        public DbSet<Comentario> Comenatarios { get; set; }
        public DbSet<AutorLibro> AutoresLibros { get; set; }
    }
}