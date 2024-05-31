using Forum.Domain.Accounts;
using Forum.Domain.Roles;
using Forum.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Forum.Persistence.Seed
{
    public static class AccountSeed
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<DataContext>();
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<Account>>();
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
                Migrate(context);
                await SeedRoles(context, roleManager);

                await SeedAdmin(context, userManager);

            }
        }
        private static void Migrate(DataContext context)
        {
            context.Database.Migrate();
        }

        private static async Task SeedRoles(DataContext context, RoleManager<IdentityRole<Guid>> userManager)
        {
            userManager.CreateAsync(new IdentityRole<Guid>(Roles.Customer)).GetAwaiter().GetResult();
            userManager.CreateAsync(new IdentityRole<Guid>(Roles.Admin)).GetAwaiter().GetResult();
            await context.SaveChangesAsync();

        }
        private static async Task SeedAdmin(DataContext context, UserManager<Account> userManager)
        {
            var admin = new Account
            {
                UserName = "Admin",
                Email = "Admin@gmail.com",
                PhoneNumber = "574045774",
                Gender = true,
               Status = Domain.Enums.Status.Active
            };

            await userManager.AddPasswordAsync(admin, "Tbctbc123-");

            context.Accounts.Add(admin);

            var adminRole = context.Roles.FirstOrDefault(r => r.Name == Roles.Admin);
            if (adminRole != null)
            {
                context.UserRoles.Add(new IdentityUserRole<Guid> { UserId = admin.Id, RoleId = adminRole.Id });
                await context.SaveChangesAsync();

            }
        }
       
    }
}
