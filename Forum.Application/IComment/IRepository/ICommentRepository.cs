using Forum.Application.CommentModel;
using Forum.Application.IRepository;
using Forum.Domain.Accounts;
using Forum.Domain.Articles;
using Forum.Domain.Comments;

namespace Forum.Application.IComment.IRepository
{
    public interface ICommentRepository : IBaseRepository<Comment>
    {
        Task Create(CommentRequestModel request,Account account,Article article,CancellationToken cancellationToken = default);

    }
}
