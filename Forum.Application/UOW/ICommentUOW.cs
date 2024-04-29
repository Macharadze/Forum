using Forum.Application.IArticle.IRepository;
using Forum.Application.IComment.IRepository;

namespace Forum.Application.UOW
{
    public interface ICommentUOW : IUnitOfWork
    {
        ICommentRepository _commentRepository { get; }
        IArticleRepository _articleRepository { get; }
    }
}
