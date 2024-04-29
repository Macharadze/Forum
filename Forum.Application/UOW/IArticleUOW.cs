using Forum.Application.IArticle.IRepository;

namespace Forum.Application.UOW
{
    public interface IArticleUOW : IUnitOfWork
    {
        IArticleRepository _articleRepository { get; }
    }
}
