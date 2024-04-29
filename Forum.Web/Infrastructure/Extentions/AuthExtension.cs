using Microsoft.AspNetCore.Authentication.Cookies;

namespace Forum.Web.Infrastructure.Extentions
{
    public static class AuthExtension
    {
        public static void AddCustomCookieAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.LoginPath = "/Account/Login";
                        options.LogoutPath = "/Account/Logout";
                        options.Cookie.Name = "AuthCookie";
                    });
        }
    }
}
