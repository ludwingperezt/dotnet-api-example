
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
/**
* En esta clase se hace la configuración de servicios y middlewares.
* No es obligatorio usarla, porque se puede usar la clase Program.cs
* sin embargo es algo recomendado.
*/
namespace WebApiAutores
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        /**
        * En esta función se van a configurar los servicios
        */
        public void ConfigureServices(IServiceCollection services)
        {
            // Esta línea se agrega para que al serialializar un objeto que referencia a una llave foránea
            // no se haga una referencia ciclica.
            services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            // Aqui se configura el acceso a la base de datos usando el string de conexión
            // configurado en el archivo appsettings.Development.json bajo el key "defaultConnection"
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        /** 
        * Configuración de la app
        */
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger) 
        {
            // ================================ INICIO de ejemplos de middlewares =====================================

            // Aqui se configuran todos los middlewares
            // Los middlewares son los que comienzan por Use

            // El objeto IApplicationBuilder es el que permite configurar la aplicación

            

            // ***** Ejemplo 3:
            // En este otro ejemplo la funcionalidad del middleware es enviar a log todas las respuestas
            // a las peticiones HTTP devueltas por la API.  
            // Se coloca aquí porque en este punto se ejecuta el último middleware antes de enviar la 
            // respuesta al cliente.
            // Para este caso se usa Use() en lugar de Run() porque NO se quiere detener toda la tubería
            // de peticiones.
            app.Use(async (contexto, siguiente) => {
                
                // Para leer la respuesta se necesita usar un MemoryStream para guardar
                // en memoria el cuerpo de la petición porque esa respuesta está en un buffer
                // por lo que es necesario copiarla, escribirla en un string y luego devolverla
                // al buffer para que sea enviada al cliente.

                using (var ms = new MemoryStream())
                {
                    var cuerpoOriginalRespuesta = contexto.Response.Body;
                    contexto.Response.Body = ms;

                    // Con esto se permite a la tubería de procesos continuar:
                    await siguiente.Invoke();

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
            });


            // ***** Ejemplo 1:
            // El siguiente es un middleware de ejemplo que atrapa todas las peticiones HTTP
            // y que retorna un string.
            // Este middleware de ejemplo será el primero en ejecutar.
            
            // Con app.Run() se puede ejecutar un middleware y cortar la ejecución de los
            // siguientes middlewares.
            app.Run(async contexto => {
                await contexto.Response.WriteAsync("Se está interceptando la tubería de peticiones HTTP!");
            });

            // ***** Ejemplo 2:
            // Si queremos que un middleware se ejecute para una ruta específica.
            // Con Map() se hace una bifurcación de la tubería de procesos
            app.Map("/ruta1", app => {
                app.Run(async contexto => {
                    await contexto.Response.WriteAsync("Este middleware solo se ejecuta para /ruta1");
                });
            });

            
            // ================================ FIN de ejemplos de middlewares =====================================

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            // app.MapControllers();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}