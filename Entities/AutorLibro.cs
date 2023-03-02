using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiAutores.Entities
{
    public class AutorLibro
    {
        // Referencias a otras entidades.
        public int LibroId { get; set; }

        public int AutorId { get; set; }

        // La propiedad Orden es para determinar quién es el autor
        // principal de un libro y los autores secundarios.
        public int Orden { get; set; }

        // Propiedades de navegación:
        public Libro Libro { get; set; }

        public Autor Autor { get; set; }
        
    }
}