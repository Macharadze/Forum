
using Forum.Api.Infrastructure.Middlewares;

namespace Forum.Api.Infrastructure.Extensions
{
    public static class CultureExtention
    {
        public static IApplicationBuilder UseCultureMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<CultureMiddleware>();
            return app;
        }
    }
}
