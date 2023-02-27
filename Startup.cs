
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Filtros;
using WebApiAutores.Middlewares;
using WebApiAutores.Servicios;
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
            services.AddControllers(opciones => {
                opciones.Filters.Add(typeof(FiltroDeExcepcion));  // Ejemplo de cómo aplicar un filtro de excepción global.
            }).AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            // Aqui se configura el acceso a la base de datos usando el string de conexión
            // configurado en el archivo appsettings.Development.json bajo el key "defaultConnection"
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));

            // Aqui se registra el filtro personalizado como Transient porque no se necesita ninguna
            // clase de manejo de estado
            services.AddTransient<MiFiltroAccion>();

            // Ejemplo de uso de ejecución de un servicio recurrente. En el ejemplo se hace que un servicio
            // ejecute una tarea al iniciar la aplicación y también cuando finaliza.
            services.AddHostedService<EscribirEnArchivo>();

            // Ejemplo de uso de los servicios para el filtro de caché:
            services.AddResponseCaching();

            // Ejemplo de filtro de autenticacion
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

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

            // Este filtro es importante tenerlo en este punto antes de UseEnpoints
            // porque la autorización se debe configurar antes de mapear los controlers con
            // las acciones.
            app.UseAuthorization();

            // app.MapControllers();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}