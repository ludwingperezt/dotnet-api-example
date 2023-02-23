using WebApiAutores;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Aquí se configuran por defecto los servicios pero como se está utilizando
// la clase Startup, entonces todo el código que está comentado se ha trasladado
// a esa clase. No es obligatorio hacerlo así, solo se ha hecho de esta forma por
// comodidad.
// builder.Services.AddControllers();
// // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// Instanciar la clase de configuración
var startup = new Startup(builder.Configuration);

// mandar a configurar los servicios
startup.ConfigureServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();

// app.UseAuthorization();

// app.MapControllers();

// Instanciar un ILogger y luego inyectarla a la clase de startup para que haga el loggeo
// de la respuesta a través de un middleware.
var serviceLogger = (ILogger<Startup>) app.Services.GetService(typeof(ILogger<Startup>));

// Ejecutar el resto de la configuración
startup.Configure(app, app.Environment, serviceLogger);

app.Run();
