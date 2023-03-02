
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Filtros;

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

        // Este elemento permite acceder a los archivos de appsettings.
        public IConfiguration Configuration { get; }


        /**
        * En esta función se van a configurar los servicios para iniciar la inyección de dependencias de cada uno.
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
            // En este caso se está utilizando EF core para que trabaje con SqlServer
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("defaultConnection"))); 

            // Ejemplo de filtro de autenticacion
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // Aqui se inicia y configura el automapper para el proyecto.
            services.AddAutoMapper(typeof(Startup));
        }

        /** 
        * Configuración de la app
        */
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger) 
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

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