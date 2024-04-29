using Forum.Application.IUser;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Forum.Infrastructure.BackgroundWorkers
{
    public class BanAccount : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public BanAccount(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var serviceScope = _serviceProvider.CreateScope())
            {
                var userService = serviceScope.ServiceProvider.GetService<IUserService>();

                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);

                    await userService!.UnBanAccount();
                }
            }

        }
    }

}
