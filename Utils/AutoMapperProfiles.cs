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

            CreateMap<Autor, AutorDTOConLibros>()
                .ForMember(autorDto => autorDto.Libros, opciones => opciones.MapFrom(MapAutorDTOLibros));

            // Aqui se especifica una regla específica de mapeo para el campo AutoresIds
            // de la clase LibroCreacionDTO, que convierte la lista de ID's de
            // autores recibida (que es una lista de enteros) y la convierte
            // en objetos AutorLibro listos para ser insertados en la db.
            CreateMap<LibroCreacionDTO, Libro>().ForMember(libro => libro.AutoresLibros, opciones => opciones.MapFrom(MapAutoresLibros));

            CreateMap<Libro, LibroDTO>();
            
            // Configurar un mapeo de la clase Libro hacia la clase LibroDTO
            // para presentar los autores de un libro a través de la relación
            // muchos a muchos AutorLibro
            CreateMap<Libro, LibroDTOConAutores>()
                .ForMember(libroDto => libroDto.Autores, opciones => opciones.MapFrom(MapLibroDTOAutores));

            // Con ReverseMap se configura también el mapeo inverso.
            CreateMap<LibroPatchDTO, Libro>().ReverseMap();

            CreateMap<ComentarioCreacionDTO, Comentario>();

            CreateMap<Comentario, ComentarioDTO>();
        }

        // Esta función hace un mapeo entre la propiedad AutoresIds que es una
        // lista de enteros (los IDs de los libros) hacia objetos de tipo AutorLibro
        // para que estén listos para la inserción en la base de datos.
        private List<AutorLibro> MapAutoresLibros(LibroCreacionDTO libroCreacion, Libro libro)
        {
            var resultado = new List<AutorLibro>();

            if (libroCreacion.AutoresIds == null) { return resultado; }

            foreach (var autorId in libroCreacion.AutoresIds)
            {
                // El ID de libro no se pone aquí porque eso lo hace EF Core
                // al momento de insertar el libro.
                resultado.Add(new AutorLibro() { AutorId = autorId });
            }

            return resultado;
        }

        // En esta función se mapean desde un objeto Libro hacia un objeto LibroDTO
        // los autores de un libro pasando por la tabla intermedia AutorLibro
        private List<AutorDTO> MapLibroDTOAutores(Libro libro, LibroDTO libroDTO)
        {
            var resultado = new List<AutorDTO>();

            if (libro.AutoresLibros == null) { return resultado; }

            foreach (var autorLibro in libro.AutoresLibros)
            {
                resultado.Add(new AutorDTO() 
                {
                    Id = autorLibro.AutorId,
                    Nombre = autorLibro.Autor.Nombre
                });
            }

            return resultado;
        }

        private List<LibroDTO> MapAutorDTOLibros(Autor autor, AutorDTO autorDto)
        {
            var resultado = new List<LibroDTO>();

            if (autor.AutoresLibros == null) { return resultado; }

            foreach (var autorLibro in autor.AutoresLibros)
            {
                resultado.Add(new LibroDTO()
                {
                    Id = autorLibro.LibroId,
                    Titulo = autorLibro.Libro.Titulo
                });
            }

            return resultado;
        }
    }
}