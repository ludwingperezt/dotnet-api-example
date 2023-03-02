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
    }
}