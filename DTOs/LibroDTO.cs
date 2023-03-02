using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiAutores.DTOs
{
    public class LibroDTO
    {
        public int Id { get; set; }

        public string Titulo { get; set; }

        // Si lo que se desea es hacer Lazy Loading (recomendado), entonces se omite
        // la propiedad Comentarios.  También se debe quitar el Include() en el query
        // que obtiene un libro (si se quita el Include() pero no se quita esta propiedad
        // entonces siempre se retorna una lista vacía).
        // public List<ComentarioDTO> Comentarios { get; set; }
    }
}