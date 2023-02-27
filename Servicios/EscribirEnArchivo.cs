using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiAutores.Servicios
{
    public class EscribirEnArchivo : IHostedService
    {
        private readonly IWebHostEnvironment env;
        private readonly string nombreArchivo = "Archivo1.txt";

        private Timer timer;

        public EscribirEnArchivo(IWebHostEnvironment env)
        {
            this.env = env;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Esto se ejecuta al iniciar la aplicación
            
            // iniciar el timer:
            // El timer ejecuta la función DoWork, sin usar estado, sin delay desde
            // el inicio del timer, cada 5 segundos.
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

            Escribir("Proceso iniciado");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Esto se ejecuta al detener la aplicación.
            // NOTA IMPORTANTE: Este código no necesariamente se va ejecutar ya que
            // en las situaciones excepcionales (en caso de una detención repentina 
            // debido a un error catastrófico) es posible que no haya tiempo de ejecutar
            // este código.

            timer.Dispose();  // Detener el timer.
            
            Escribir("Proceso finalizado");
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            Escribir("Proceso en ejecucion: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
        }

        private void Escribir(string mensaje) 
        {
            // La ruta wwwroot/ es una ruta especial en .net core en la cual
            //  se sirven los archivos estáticos.
            var ruta = $@"{env.ContentRootPath}/wwwroot/{nombreArchivo}";
            using (StreamWriter wr = new StreamWriter(ruta, append: true))
            {
                wr.WriteLine(mensaje);
            }
        }
    }
}