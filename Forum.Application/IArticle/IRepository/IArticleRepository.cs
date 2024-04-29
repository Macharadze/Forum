
using Forum.Application.ArticleModel;
using Forum.Application.IRepository;
using Forum.Domain.Accounts;
using Forum.Domain.Articles;
using Forum.Domain.Comments;

namespace Forum.Application.IArticle.IRepository
{
    public interface IArticleRepository : IBaseRepository<Article>
    {
        Task Add(ArticleRequestModel entity,Account accounXt,string path,CancellationToken cancellationToken = default);
        Task Update(string id,ArticleRequestModel requestModel, CancellationToken cancellationToken = default);
        Task<string> UploadImage(string title, CancellationToken token = default);
        Task<IEnumerable<Article>> Search(string search,CancellationToken cancellationToken = default);

        Task<IEnumerable<Article>> PendingArticles(CancellationToken cancellationToken = default);
        Task<IEnumerable<Comment>> Comments(String ID,CancellationToken cancellationToken = default);
        Task<int> GetNumberOfPosts(Account account, CancellationToken cancellationToken = default);
        Task DeActive(CancellationToken cancellationToken);



    }
}
