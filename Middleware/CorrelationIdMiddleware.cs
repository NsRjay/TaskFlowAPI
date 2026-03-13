using System.Net.Cache;
using Serilog.Context;
namespace TaskFlowAPI.Middleware
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private const string HeaderName= "X-Correlation-ID";
        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next=next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId=context.Request.Headers[HeaderName].FirstOrDefault();
            if (string.IsNullOrEmpty(correlationId))
            {
                correlationId=Guid.NewGuid().ToString();
            }
            context.Response.Headers[HeaderName]=correlationId;
            using (LogContext.PushProperty("CorrelationId",correlationId))
            {
                await _next(context);
            }
        }
    }
}