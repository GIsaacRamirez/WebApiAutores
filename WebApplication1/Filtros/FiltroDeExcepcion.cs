using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiAutores.Filtros
{
    public class FiltroDeExcepcion: ExceptionFilterAttribute
    {
        private readonly ILogger<MiFiltroDeAccion> logger;
        public FiltroDeExcepcion(ILogger<MiFiltroDeAccion> logger)
        {
            this.logger = logger;
        }
        public override void OnException(ExceptionContext context)
        {
            logger.LogInformation(context.Exception, context.Exception.Message);
            base.OnException(context);
        }

    }


}
