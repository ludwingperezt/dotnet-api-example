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
    public class Autor:IValidatableObject
    {
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
        [StringLength(maximumLength: 50, ErrorMessage = "El campo {0} no debe tener más de {1} caracteres")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }

        public List<Libro> Libros { get; set; }

        // Con Range() se valida que el campo esté entre los números especificados
        // cuando el valor es numérico.
        // NotMapped es usado para que el campo no sea creado en la base de datos.
        [Range(18, 120)]
        [NotMapped]
        public int Edad { get; set; }

        // CreditCard valida la numeración de una tarjeta de credito
        [CreditCard]
        [NotMapped]
        public string CreditCard { get; set; }

        // Url valida que el valor sea una URL correcta.
        [Url]
        [NotMapped]
        public string URL { get; set; }

        public int Menor { get; set; }
        public int Mayor { get; set; }

        // Para la validación a nivel de modelo se implementa el método Validate()
        // Para que esta validación se ejecute antes se deben pasar todas las reglas de
        // validación asignadas a cada campo.
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // En este caso a manera de ejemplo se repite la validación de la primera letra mayúscula
            if (!string.IsNullOrEmpty(Nombre))
            {
                var primeraLetra = Nombre[0].ToString();

                if (primeraLetra != primeraLetra.ToUpper())
                {
                    // Al generar el resultado a retornar se envía como parámetro el nombre de la propiedad.
                    // Se usa yield para llenar el Enumerable de la lista de errores.
                    yield return new ValidationResult("La primera letra del nombre debe ser mayuscula", 
                        new string[] { nameof(Nombre) });
                }
            }

            // Aqui se hace una validación de dos campos del objeto.
            if (Menor > Mayor) 
            {
                yield return new ValidationResult("Este valor no puede ser mas grande que el campo Mayor",
                    new string[] { nameof(Menor) });
            }
        }
    }
}