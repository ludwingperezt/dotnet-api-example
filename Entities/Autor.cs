using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiAutores.Entities
{
    public class Autor
    {
        public int Id { get; set; }

        // El attibute Required indica que el dato es obligatorio.
        // También se modifica el mensaje de error por uno personalizado.
        // El uso de {0} coloca el nombre del campo en el string cuando sea
        // requerido.
        // Pueden usarse varias validaciones sobre el campo, como por ejemplo en
        // este caso se usa StringLength para aceptar hasta un máximo de caracteres para
        // el nombre.
        [Required(ErrorMessage ="El campo {0} es requerido")]
        [StringLength(maximumLength: 50, ErrorMessage = "El campo {0} no debe tener más de {1} caracteres")]
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
        public int MyProperty { get; set; }

        // Url valida que el valor sea una URL correcta.
        [Url]
        [NotMapped]
        public string URL { get; set; }
    }
}