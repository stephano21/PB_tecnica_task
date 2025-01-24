using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using System.Net;
using Tasks;

namespace Tasks.Filters
{
    public class FilterExeption: ExceptionFilterAttribute
    {
        private readonly ILogger<FilterExeption> logger;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration configuration;
        public FilterExeption(ILogger<FilterExeption> logger, ApplicationDbContext context, IConfiguration configuration)
        {
            this.configuration = configuration;
            this.logger = logger;
            _context = context;
        }
        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            // Capturar información específica para los logs
            var logMessage = $"\n\n----------------------------ERROR-{context.ActionDescriptor.DisplayName}----------------------------------\n" +
                             $"Request ID: {Activity.Current?.Id}\n" +
                             $"Request TraceIdentifier: {context.HttpContext.TraceIdentifier}\n" +
                             $"Date: {DateTime.Now}\n" +
                             $"Controller: {context.ActionDescriptor.RouteValues["controller"]}\n" +
                             $"Endpoint: {context.ActionDescriptor.DisplayName}\n" +
                             $"Message: {context.Exception.Message}\n" +
                             $"StackTrace: {context.Exception.StackTrace}\n" +
                             $"Usuario: {context.HttpContext.Request.Cookies["username"]}\n" +
                             $"InnerException: {context.Exception?.InnerException?.ToString() ?? ""}\n" +
                             "-------------------------------------------------------------------------\n" +
                             "-------------------------------------------------------------------------\n\n";

            // Registrar en el archivo de registro usando Serilog o cualquier otra configuración que utilices
            logger.LogError(logMessage);

            // Configurar la excepción en el contexto
            context.ExceptionHandled = true;
            context.Result = new ObjectResult(context.Exception.Message)
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };

            base.OnException(context);
        }
    }
}
