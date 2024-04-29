using Forum.Api.Models;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Net;

namespace Forum.Api.Infrastructure.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandlingMiddleware(RequestDelegate request)
        {
            _next = request;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                
               Log.Error(ex, $"Something went wrong: {ex.Message}");

                await HandleExceptionAsync(context, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error "
            }.ToString());
        }
    }
}
