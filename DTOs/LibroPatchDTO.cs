using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiAutores.DTOs
{
    public class LibroPatchDTO
    {
        // [PrimeraLetraMayuscula]
        [StringLength(maximumLength: 120)]
        [Required]
        public string Titulo { get; set; }

        public DateTime FechaPublicacion { get; set; }
    }
}