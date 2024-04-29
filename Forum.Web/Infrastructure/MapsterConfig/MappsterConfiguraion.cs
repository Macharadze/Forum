using Forum.Application.AccountModel;
using Forum.Application.ArticleModel;
using Forum.Application.CommentModel;
using Forum.Domain.Accounts;
using Forum.Domain.Articles;
using Forum.Web.Models;
using Mapster;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;

namespace Forum.Web.Infrastructure.MapsterConfig
{
    public static class MappsterConfiguraion
    {
        public static void RegisterMaps(this IServiceCollection services)
        {

            TypeAdapterConfig<Article, ArticleViewModelWithIDs>
                       .NewConfig()
                       .Map(dest => dest.Comments, src =>
                        src.Comments.Select(comment => new CommentViewModel()
                        {
                            Id = comment.Id,
                            Content = comment.Content,
                            UserName = comment.User.UserName,
                            Path = comment.User.Path,
                            Date = comment.CreatedAt,
                            Email = comment.User.Email
                        }).ToList()
                       );

            TypeAdapterConfig<Article, ArticleResponse>
                       .NewConfig()
                       .Map(dest => dest.Comments, src =>
                        src.Comments.Select(comment => new CommentViewModel()
                        {
                            Content = comment.Content,
                            UserName = comment.User.UserName,
                            Path = comment.User.Path,
                            Date = comment.CreatedAt,
                            Email = comment.User.Email
                        }).ToList()
                       );
            TypeAdapterConfig<Account, AccountResponseModel>
                .NewConfig()
                .Map(dest => dest.ArticleResponses, src =>
                src.Articles.Select(article => new ArticleResponse()
                {
                    Content = article.Content,
                    Title = article.Title,
                    Id = article.Id,
                    Comments = article.Comments.Select(comment => new CommentResponseModel()
                    {
                       
                        Content = comment.Content,
                        UserName = comment.User.UserName,
                        Path = comment.User.Path,
                        Date = comment.CreatedAt,
                        
                    }
                    ).ToList(),
                }
                ).ToList()
                );




        }
    }
}
