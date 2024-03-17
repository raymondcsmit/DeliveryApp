using Microsoft.AspNetCore.Http;
using Serilog;
using System.Net;

namespace Core
{

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                LogException(ex);
                await HandleExceptionAsync(context, ex);
            }
        }

        private void LogException(Exception ex)
        {
            Log.Logger.Error(ex, "An unhandled exception has occurred.");
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var result = System.Text.Json.JsonSerializer.Serialize(new
            {
                StatusCode = context.Response.StatusCode,
                Message = $"Internal Server Error from the custom middleware. Detail:{context.Response.Body}"
            });

            return context.Response.WriteAsync(result);
        }
    }

}
