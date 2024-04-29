using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Forum.Api.Infrastructure.Extensions
{
    public static class HealthCheckExension
    {
        public static void ConfigureHealthChecks(this IServiceCollection services, string connection)
        {
            services.AddHealthChecks()
                .AddSqlServer(connection, healthQuery: "select 1", name: "ForumDatabase", failureStatus: HealthStatus.Unhealthy, tags: new[] { "Feedback", "Database" });
        }
    }
}
