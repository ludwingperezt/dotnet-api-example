using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiAutores.Entities
{
    public class Libro
    {
        public int Id { get; set; }
        
        // [PrimeraLetraMayuscula]
        [Required]
        [StringLength(maximumLength: 120)]
        public string Titulo { get; set; }

        // Propiedad de navegación: Con esta referencia se pueden obtener
        // los comentarios de un libro específico.
        public List<Comentario> Comentarios { get; set; }

        public List<AutorLibro> AutoresLibros { get; set; }
        
    }
}