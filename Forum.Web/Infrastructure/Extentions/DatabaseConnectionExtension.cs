using Forum.Domain.Accounts;
using Forum.Persistence.ConfigurationsAppsettingJson;
using Forum.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Forum.Web.Infrastructure.Extentions
{
    public static class DatabaseConnectionExtension
    {
        public static void AddIdentity(this IServiceCollection services)
        {
            services.AddIdentity<Account, IdentityRole<Guid>>(option =>
            {
                option.Password.RequireDigit = true;
                option.Password.RequireUppercase = true;
                option.Password.RequireLowercase = true;
                option.Password.RequireNonAlphanumeric = true;
                option.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<DataContext>()
                        .AddDefaultTokenProviders();
        }
    }
}
