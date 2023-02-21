using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

/**
Ejemplo de creación de un validador personalizado.
*/
namespace WebApiAutores.validators
{
    public class PrimeraLetraMayusculaAttribute:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // En esta función se crea la lógica de validación
            // value es el valor a validar
            // en validationContext se tiene acceso a otros valores, como por ejemplo el objeto completo
            // al que pertenece el valor.

            // En este caso se considera un valor aceptado si es nulo ya que de ser así
            // se quiere evitar que haya solapamiento con el validador Required.
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            var primeraLetra = value.ToString()[0].ToString();
            if (primeraLetra != primeraLetra.ToUpper())
            {
                return new ValidationResult("La primera letra debe ser mayúscula");
            }
            return ValidationResult.Success;
        }
    }
}