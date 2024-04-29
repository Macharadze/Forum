using Forum.Application.ArticleModel;
using Forum.Domain.Accounts;
using Forum.Domain.Articles;
using Forum.Domain.Comments;

namespace Forum.Application.IArticle
{
   public interface IArticleService
    {
        Task<IEnumerable<Article>> GetAll(CancellationToken cancellationToken);
        Task<IEnumerable<Article>> PendingArticle(CancellationToken cancellationToken);
        Task<Article> GetArticle(CancellationToken cancellationToken, string id);
        Task Create(CancellationToken cancellationToken, ArticleRequestModel article);
        Task Update(CancellationToken cancellationToken, string id,ArticleRequestModel requestModel);
        Task Delete(CancellationToken cancellationToken, string id);
        Task DeActive(CancellationToken cancellationToken);
        Task<IEnumerable<Article>> GetAllArticlesByEmail(string email, CancellationToken cancellationToken = default);

        Task<IEnumerable<Article>> Search(string search, CancellationToken cancellationToken = default);

        Task<IEnumerable<Comment>> Comments(string ID, CancellationToken cancellationToken = default);

        Task<Account> Owner(CancellationToken cancellationToken);
        
    }
}
