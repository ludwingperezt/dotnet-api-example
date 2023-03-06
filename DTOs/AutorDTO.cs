using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiAutores.DTOs
{
    // Esta es una clase DTO para retornar datos a los clientes de la API
    public class AutorDTO
    {
        public int Id  { get; set; }

        public string Nombre { get; set; }

        // La lista de libros se trasladó a AutorDTOConLibros para que al listar
        // los libros de un autor, el campo "autores" de cada libro NO aparezca en
        // null.
        // public List<LibroDTO>  Libros { get; set; }
    }
}