using Forum.Application.IArticle.IRepository;
using Forum.Application.IUser.IRepository;
using Forum.Application.UOW;
using Forum.Persistence.Context;

namespace Forum.Infrastructure.UnitOfWorks
{
    public class ArticleUOW : UnitOFWork, IArticleUOW
    {
        public ArticleUOW(DataContext dbContext,IArticleRepository articleRepository,IUserRepository userRepository) : base(dbContext)
        {
            _articleRepository = articleRepository;
            _userRepository = userRepository;
        }

        public IArticleRepository _articleRepository { get; }

        public IUserRepository _userRepository { get; }
    }
}
