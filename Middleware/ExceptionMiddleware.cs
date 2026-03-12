using System.Net;
using System.Reflection.Metadata;
using System.Text.Json;

namespace TaskFlowAPI.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next=next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

            }
            catch (Exception ex)
            {
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