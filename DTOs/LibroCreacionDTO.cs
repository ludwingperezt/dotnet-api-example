using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiAutores.DTOs
{
    public class LibroCreacionDTO
    {
        // [PrimeraLetraMayuscula]
        [StringLength(maximumLength: 120)]
        public string Titulo { get; set; }

        public List<int> AutoresIds { get; set; }
    }
}