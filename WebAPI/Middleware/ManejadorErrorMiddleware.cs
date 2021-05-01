using Aplicacion.ManejadorError;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Middleware
{
    public class ManejadorErrorMiddleware
    {

        private readonly RequestDelegate _requestDelegate;
        private readonly ILogger<ManejadorErrorMiddleware> _logger;

        public ManejadorErrorMiddleware(RequestDelegate requestDelegate, ILogger<ManejadorErrorMiddleware> logger)
        {
            _requestDelegate = requestDelegate;
            _logger = logger;
        }


        public async Task Invoke(HttpContext httpContext)
        {

            try
            {
                await _requestDelegate(httpContext);
            }
            catch (Exception ex)
            {
                await ManejadorExcepcionesAsincrono(httpContext, ex, _logger);
            }

        }

        private async Task ManejadorExcepcionesAsincrono(HttpContext httpContext, Exception ex, ILogger<ManejadorErrorMiddleware> logger)
        {
            object errores = null;
            switch (ex)
            {

                case ManejadorExcepcion me:
                    logger.LogError(ex, "Manejador error");
                    errores = me._errores;
                    httpContext.Response.StatusCode = (int)me._statusCode;
                    break;

                case Exception e:
                    logger.LogError(ex, "Manejo de errores");
                    errores = string.IsNullOrWhiteSpace(ex.Message) ? "Error" : ex.Message;
                    break;

            }

            httpContext.Response.ContentType = "application/json";
            if (errores != null)
            {

                var resultados = JsonConvert.SerializeObject(new { errores });
                await httpContext.Response.WriteAsync(resultados);
            }
        }
    }
}
