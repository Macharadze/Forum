using Forum.Application.CommentModel;
using Forum.Application.Exceptions;
using Forum.Application.UOW;
using Forum.Domain.Accounts;
using Forum.Domain.Articles;
using Forum.Domain.Comments;

namespace Forum.Application.IComment
{
    public class CommentService : ICommentService
    {
        private readonly ICommentUOW _uow;

        public CommentService(ICommentUOW uow)
        {
            _uow = uow;
        }

        public async Task CreateAsync(CommentRequestModel reques, string articleID, CancellationToken cancellationToken = default)
        {
            var account = await OwnerAccount(cancellationToken);
            var article = await OwnerArticle(articleID, cancellationToken);

            if (article.Status == Domain.Enums.Status.Inactive)
            {
                 throw new CommentIsNotAllowedException("");
            }
            await _uow._commentRepository.Create(reques, account, article, cancellationToken);
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            await _uow._commentRepository.Delete(id, cancellationToken);
        }

        public async Task<Comment> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _uow._commentRepository.GetById(id, cancellationToken);

        }


        public async Task<Account> OwnerAccount(CancellationToken cancellationToken = default)
        {

            return await _uow._userRepository.GetCurrentUser(cancellationToken);

        }

        public async Task<Article> OwnerArticle(string ID, CancellationToken cancellationToken = default)
        {

            return await _uow._articleRepository.GetById(ID, cancellationToken);

        }
    }
}
