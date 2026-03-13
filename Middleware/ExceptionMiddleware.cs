using System.Net;
using System.Reflection.Metadata;
using System.Text.Json;
using TaskFlowAPI.Services;

namespace TaskFlowAPI.Middleware
{
    public class ExceptionMiddleware
    {
        
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next,ILogger<ExceptionMiddleware> logger)
        {
            _next=next;
            _logger=logger;
            
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Unhandled exception occured at {Path}",context.Request.Path);
                await HandleException(context,ex);
            }
        }
        private static Task HandleException(HttpContext context, Exception exception)
        {
            
            context.Response.ContentType="application/json";
            context.Response.StatusCode= (int)HttpStatusCode.InternalServerError;
            var response= new {success="false",message=exception.Message};//"An unexpected error has occured"};
            var json=JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(json);
        }
    }
}