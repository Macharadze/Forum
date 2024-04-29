using Forum.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace Forum.Web.Infrastructure.Middlewares
{
    public class ErrorHandilngMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandilngMiddleware(RequestDelegate request)
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
                Log.Error(ex, "An unhandled exception occurred");


                string routeWhereExeptionOccurred = context.Request.Path;

                var path = JsonConvert.SerializeObject(routeWhereExeptionOccurred);
                var problemDetails = new ProblemDetails
                {
                    Status = context.Response.StatusCode,
                    Title = "An error occurred",
                    Detail = "Sorry, an error occurred while processing your request."
                };

                var result = new ErrorViewModel
                {
                    Path = path,
                    ProblemDetails = problemDetails
                   
                };

              
                if (ex is AggregateException ex2)
                {
                    var message = ex2.InnerExceptions.Select(
                        e => e.Message).ToList();
                    result.Errors = message;
                    string messageJson = JsonConvert.SerializeObject(result);
                    context.Items["ErrorMessageJson"] = messageJson;
                }
                else
                {
                    string message = ex.Message;
                    result.Errors = new List<string> { message };
                    string messageJson = JsonConvert.SerializeObject(result);
                    context.Items["ErrorMessageJson"] = messageJson;

                }

                await HandleErrors(context);

            }
            static async Task HandleErrors(HttpContext context)
            {
                string messagesJson = context?.Items?["ErrorMessageJson"] as string;

                string redtrectUrl = $"/Home/Error?messageJson={messagesJson}";
                context?.Response.Redirect(redtrectUrl);
                await Task.CompletedTask;
            }
        }
    }
}
