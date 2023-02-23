using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiAutores.Middlewares
{
    // Aqui se genera una clase estática porque lo que se va a crear es un método
    // de extensión, y eso solo es posible a través de clases estáticas.
    // Esta es una clase de utilidad que se usa para invocar un middleware de forma más
    // simple en la clase Startup.
    public static class LoggearRespuestaHTTPMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoggearRespuestaHTTP(this IApplicationBuilder app) 
        {
            return app.UseMiddleware<LoggearRespuestaHTTPMiddleware>();
        }
    }

    public class LoggearRespuestaHTTPMiddleware
    {
        private readonly RequestDelegate siguiente;
        private readonly ILogger<LoggearRespuestaHTTPMiddleware> logger;

        // Con el RequestDelegate se indicará que se quieren invocar los siguientes
        // middlewares de la tubería
        public LoggearRespuestaHTTPMiddleware(RequestDelegate siguiente, ILogger<LoggearRespuestaHTTPMiddleware> logger)
        {
            this.siguiente = siguiente;
            this.logger = logger;
        }

        // Toda clase Middleware debe tener un método público llamado
        // Invoke o InvokeAsync.  Estos métodos deben retornar siempre una tarea
        // y su primer parámetro debe ser un HTTP Context.
        public async Task InvokeAsync(HttpContext contexto) 
        {
            using (var ms = new MemoryStream())
            {
                var cuerpoOriginalRespuesta = contexto.Response.Body;
                contexto.Response.Body = ms;

                // Con esto se permite a la tubería de procesos continuar:
                await siguiente(contexto);

                // LO que venga después de esta línea se ejecutará cuando los 
                // middlewares posteriores estén devolviendo una respuesta
                ms.Seek(0, SeekOrigin.Begin); // IR al inicio del stream
                
                // Se lee el memory stream hasta el final, lo que permite guardar
                // en la variable string cualquiera que sea la respuesta que se
                // esté enviando al cliente.
                string respuesta = new StreamReader(ms).ReadToEnd();

                // Luego se retorna el stream a la posición inicial para poder
                // enviar la respuesta correctamente al usuario.
                ms.Seek(0, SeekOrigin.Begin);

                // Toda esta manipulación del stream permite leer la respuesta, guardarla
                // y volverla a colocar como estaba para que el cliente reciba bien la
                // respuesta.
                await ms.CopyToAsync(cuerpoOriginalRespuesta);
                contexto.Response.Body = cuerpoOriginalRespuesta;

                // Escribir la respuesta en el log.
                logger.LogInformation(respuesta);

            }
        }
    }
}