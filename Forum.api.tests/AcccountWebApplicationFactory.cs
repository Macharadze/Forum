using Forum.Application.IArticle.IRepository;
using Forum.Application.IArticle;
using Forum.Application.IBanAccount;
using Forum.Application.IComment.IRepository;
using Forum.Application.IComment;
using Forum.Application.IUser;
using Forum.Application.IUser.IRepository;
using Forum.Application.UOW;
using Forum.Infrastructure.Accounts;
using Forum.Infrastructure.Articles;
using Forum.Infrastructure.BackgroundWorker;
using Forum.Infrastructure.BackgroundWorkers;
using Forum.Infrastructure.BanAccounts;
using Forum.Infrastructure.Comments;
using Forum.Infrastructure.UnitOfWorks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Testing;
using Forum.Api;
using Microsoft.Extensions.Logging;
using Forum.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Forum.Persistence.Seed;
using Forum.Domain.Accounts;
using Microsoft.AspNetCore.Identity;

namespace Forum.api.tests
{
    public class AcccountWebApplicationFactory : WebApplicationFactory<IAPIMarker>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {

            builder.ConfigureLogging(i =>
            i.ClearProviders());
            builder.ConfigureTestServices(services => {
                var descriptor = services.SingleOrDefault(
                   d => d.ServiceType == typeof(DbContextOptions<DataContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add DataContext using a test database connection string.
                services.AddDbContext<DataContext>(options =>
                {
                    options.UseSqlServer("Server=(LocalDb)\\LocalDB; Database=ForumDatabase; Integrated Security=True; Trusted_Connection=True;");
              
                });
             

                var serviceProvider = services.BuildServiceProvider();
                using (var scope = serviceProvider.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<DataContext>();
                    db.Database.EnsureCreated();
                   //  AccountSeed.Initialize(serviceProvider);
                    // Seed the database with test data if needed.
                }
            });

     
        } }
}
