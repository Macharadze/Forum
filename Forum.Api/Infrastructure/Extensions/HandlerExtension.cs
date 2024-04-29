using Forum.Api.Infrastructure.Middlewares;

namespace Forum.Api.Infrastructure.Extensions
{
    public static class HandlerExtension
    {
        public static IApplicationBuilder UseHandlerMiddleware(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ErrorHandlingMiddleware>();
            return builder;
        }
    }
}
