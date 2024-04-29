using Forum.Application.CommentModel;
using Forum.Domain.Accounts;
using Forum.Domain.Articles;
using Forum.Domain.Comments;

namespace Forum.Application.IComment
{
    public interface ICommentService
    {
        Task<Comment> GetByIdAsync(string id, CancellationToken cancellationToken = default);
      //  Task<IEnumerable<Comment>> GetAllAsync(CancellationToken cancellationToken = default);
        Task CreateAsync(CommentRequestModel reques,string articleID,CancellationToken cancellationToken = default);
        Task DeleteAsync(string id, CancellationToken cancellationToken = default);
        Task<Account> OwnerAccount(CancellationToken cancellationToken = default);
        Task<Article> OwnerArticle(string ID, CancellationToken cancellationToken = default);

    }
}
