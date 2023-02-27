
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Middlewares;
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

            // Ejemplo de uso de los servicios para el filtro de caché:
            services.AddResponseCaching();

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

            // Forma no. 1 de usar el middleware y la más sencilla.
            // En este caso se expone cual es la clase utilizada.
            // app.UseMiddleware<LoggearRespuestaHTTPMiddleware>();

            // Forma no. 2 de usar el middleware.  Para esta forma
            // hace falta crear una clase estática de utilidad.
            app.UseLoggearRespuestaHTTP();

            
            // ================================ FIN de ejemplos de middlewares =====================================

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // Ejemplo de uso de un filtro de caché
            app.UseResponseCaching();

            app.UseAuthorization();

            // app.MapControllers();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}