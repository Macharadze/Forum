using Forum.Application.AccountModel;
using Forum.Application.ArticleModel;
using Forum.Application.CommentModel;
using Forum.Domain.Accounts;
using Forum.Domain.Articles;
using Mapster;

namespace Forum.Api.Infrastructure.MapsterConfiguration
{
    public static class MapsterConfig
    {
        public static void RegisterMaps(this IServiceCollection services)
        {
            TypeAdapterConfig<Article, ArticleResponse>
                     .NewConfig()
                     .Map(dest => dest.Comments, src =>
                      src.Comments.Select(comment => new CommentResponseModel()
                      {
                          Content = comment.Content,
                          UserName = comment.User.UserName,
                          Path = src.Path,
                          Date = comment.CreatedAt,
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
                        Path = src.Path,
                        Date = comment.CreatedAt,

                    }
                    ).ToList(),
                }
                ).ToList()
                );
        }
    }
}
