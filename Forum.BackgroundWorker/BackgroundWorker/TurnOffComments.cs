using Forum.Application.IArticle.IRepository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Forum.Infrastructure.BackgroundWorker
{
    public class TurnOffComments : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public TurnOffComments(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var serviceScope = _serviceProvider.CreateScope())
            {
                var userService = serviceScope.ServiceProvider.GetService<IArticleRepository>();

                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);

                    await userService!.DeActive(new CancellationToken());
                }
            }

        }
    }
}
