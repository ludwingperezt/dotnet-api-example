using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiAutores.validators;

namespace WebApiAutores.Entities
{
    // Para hacer una validación a nivel de modelo se debe implementar la interfaz IValidatableObject
    // Se pueden crear validaciones a nivel de modelo.
    // Esto es útil cuando se quieren validar varios campos en conjunto para un modelo.
    // Para esto es necesario que el modelo implemente la interfaz IValidatableObject
    // y que luego se implementen los métodos de la interfaz
    public class Autor // :IValidatableObject
    {
        // Para EF, si un campo se llama "Id" entonces este campo se manejará como el 
        // identificador de la tabla (llave primaria)
        public int Id { get; set; }

        // El attibute Required indica que el dato es obligatorio.
        // También se modifica el mensaje de error por uno personalizado.
        // El uso de {0} coloca el nombre del campo en el string cuando sea
        // requerido.
        // Pueden usarse varias validaciones sobre el campo, como por ejemplo en
        // este caso se usa StringLength para aceptar hasta un máximo de caracteres para
        // el nombre.
        // PrimeraLetraMayusculaAttribute es un validador personalizado. En estos casos
        // se omite el sufix Attibute.
        [Required(ErrorMessage ="El campo {0} es requerido")]
        [StringLength(maximumLength: 120, ErrorMessage = "El campo {0} no debe tener más de {1} caracteres")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
    }
}