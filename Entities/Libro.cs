using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiAutores.Entities
{
    public class Libro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }

        public int AutorId { get; set; }

        // Esta es una propiedad de navegación
        public Autor Autor { get; set; }
    }
}