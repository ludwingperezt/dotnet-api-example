using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiAutores.Entities
{
    public class Comentario
    {
        public int id { get; set; }

        public string Contenido { get; set; }

        // Referencia al libro al que pertenece el comentario
        public int LibroId { get; set; }

        // Propiedad de navegación: Con ésta es mucho más fácil
        // pasar de una entidad a otra relacionada.
        // Lo que hacen por debajo es hacer joins de manera más 
        // fácil (si se desea, porque por defecto no se carga automáticamente).
        public Libro libro { get; set; }
    }
}