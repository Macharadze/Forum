using Forum.Api.Infrastructure.Middlewares;

namespace Forum.Api.Infrastructure.Extensions
{
    public static class RequestResponseExtension
    {
        public static IApplicationBuilder UseRequestResponseMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<RequestResponseMiddleware>();
            return app;
        }
    }
}
