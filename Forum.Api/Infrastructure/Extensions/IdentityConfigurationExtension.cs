using Forum.Domain.Accounts;
using Forum.Persistence.Context;
using Microsoft.AspNetCore.Identity;

namespace Forum.Api.Infrastructure.Extensions
{
    public static class IdentityConfigurationExtension
    {
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<Account, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();
        }
    }

}
