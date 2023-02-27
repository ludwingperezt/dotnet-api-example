using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiAutores.Filtros
{
    // Esta clase es un ejemplo de un filtro de excpeción que se
    // aplica de forma global a todo el API
    public class FiltroDeExcepcion: ExceptionFilterAttribute
    {
        private readonly ILogger<FiltroDeExcepcion> logger;

        public FiltroDeExcepcion(ILogger<FiltroDeExcepcion> logger)
        {
            this.logger = logger;
        }

        public override void OnException(ExceptionContext  context)
        {
            logger.LogError("Ejemplo de filtro de excepción");
            logger.LogError(context.Exception, context.Exception.Message);
            base.OnException(context);
        }
        
    }
}