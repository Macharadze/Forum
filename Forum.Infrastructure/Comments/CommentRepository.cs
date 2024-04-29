using Forum.Application.CommentModel;
using Forum.Application.IComment.IRepository;
using Forum.Domain.Accounts;
using Forum.Domain.Articles;
using Forum.Domain.Comments;
using Forum.Infrastructure.Base;
using Forum.Persistence.Context;

namespace Forum.Infrastructure.Comments
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(DataContext context) : base(context)
        {
        }

        public async Task Create(CommentRequestModel request, Account account,Article article,CancellationToken cancellationToken = default)
        {
            var comment = new Comment()
            {
                Content = request.Content,
                Article = article,
                User = account
            };
            await base.AddAsync(cancellationToken, comment);

        }

        public async Task Delete(string ID, CancellationToken cancellationToken = default)
        {
            var target = await GetById(ID);
            if (target != null)
            {
                target.Status = Domain.Enums.Status.Deleted;
                await base.UpdateAsync(cancellationToken, target);
            }
        }

        public async Task<IEnumerable<Comment>> GetAll(CancellationToken cancellationToken = default)
        {
            return await base.GetAllAsync(cancellationToken);
        }

        public async Task<Comment> GetById(string ID, CancellationToken cancellationToken = default)
        {
            return await base.GetAsync(cancellationToken, x => x.Id.ToString() == ID);
        }

     

        public async Task Update(Comment entity, CancellationToken cancellationToken = default)
        {
            entity.ModifiedAt = DateTime.UtcNow;
            await base.UpdateAsync(cancellationToken,entity);
        }
    }

}
