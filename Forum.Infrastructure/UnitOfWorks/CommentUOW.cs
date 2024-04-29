using Forum.Application.IArticle.IRepository;
using Forum.Application.IComment.IRepository;
using Forum.Application.IUser.IRepository;
using Forum.Application.UOW;
using Forum.Persistence.Context;

namespace Forum.Infrastructure.UnitOfWorks
{
    public class CommentUOW : UnitOFWork, ICommentUOW
    {
        public CommentUOW(DataContext dbContext,ICommentRepository commentRepository,IUserRepository userRepository,IArticleRepository articleRepository) : base(dbContext)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _articleRepository = articleRepository;
        }
        public IArticleRepository _articleRepository { get; }
        public ICommentRepository _commentRepository { get; }

        public IUserRepository _userRepository { get;}
    }
}
