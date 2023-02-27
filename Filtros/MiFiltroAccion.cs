using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiAutores.Filtros
{
    // Este es un ejemplo de un filtro personalizado de acción
    // (se ejecuta antes y después de la ejecución de un endpoint)
    // Lo que hace este filtro de ejemplo es mandar a log 
    public class MiFiltroAccion : IActionFilter
    {
        private readonly ILogger<MiFiltroAccion> logger;

        public MiFiltroAccion(ILogger<MiFiltroAccion> logger)
        {
            this.logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Esto se ejecuta cuando la acción YA se EJECUTÓ
            logger.LogInformation("Filtro ejecutado DESPUES de ejecutar la acción");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Esto se ejecuta ANTES de ejecutar la acción
            logger.LogInformation("Filtro ejecutado ANTES de ejecutar la acción");
        }
    }
}