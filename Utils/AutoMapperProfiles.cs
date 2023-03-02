using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WebApiAutores.DTOs;
using WebApiAutores.Entities;

namespace WebApiAutores.Utils
{
    // Esta es una clase creada para configurar los mapeos entre clases utilizando Automapper.
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            // Esto configura el mapeo entre una clase origen a una clase destino.
            CreateMap<AutorCreacionDTO, Autor>();

            CreateMap<Autor, AutorDTO>();
        }
    }
}