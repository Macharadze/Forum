using Forum.Application.IArticle;
using Forum.Application.IArticle.IRepository;
using Forum.Application.IBanAccount;
using Forum.Application.IComment;
using Forum.Application.IComment.IRepository;
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

namespace Forum.Api.Infrastructure.Extensions
{
    public static class ServiceExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, AccounrRepository>();
            services.AddScoped<IUserService, UserSerrvice>();
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IBanAccountRepository, BanAccountRepository>();
            services.AddScoped<IUserUOW, AccountUOW>();
            services.AddScoped<IArticleUOW,ArticleUOW>();
            services.AddScoped<ICommentUOW,CommentUOW>();
            services.AddHostedService<BanAccount>();
            services.AddHostedService<TurnOffComments>();

        }
    }
}
