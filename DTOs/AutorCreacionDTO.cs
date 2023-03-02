using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApiAutores.validators;

// Este es un DTO para la creación de un Autor.
namespace WebApiAutores.DTOs
{
    public class AutorCreacionDTO
    {
        [Required(ErrorMessage ="El campo {0} es requerido")]
        [StringLength(maximumLength: 120, ErrorMessage = "El campo {0} no debe tener más de {1} caracteres")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
    }
}