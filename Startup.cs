
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
            services.AddControllers();

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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) 
        {
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