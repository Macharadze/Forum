using Castle.Core.Configuration;
using Forum.Application.ArticleModel;
using Forum.Application.Exceptions;
using Forum.Application.IArticle.IRepository;
using Forum.Domain.Accounts;
using Forum.Domain.Articles;
using Forum.Domain.Comments;
using Forum.Infrastructure.Base;
using Forum.Persistence.ConfigurationsAppsettingJson;
using Forum.Persistence.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Forum.Infrastructure.Articles
{
    public class ArticleRepository : BaseRepository<Article>, IArticleRepository
    {
        private readonly int _limit;
        private readonly IHttpContextAccessor _httpContext;
        private readonly string filePath;
        private readonly int lastComment;

        public ArticleRepository(DataContext context,IOptions<LimitConfiguration> options,IHttpContextAccessor httpContextAccessor,IOptions<ImagesAddress> address) : base(context)
        {
          _limit = options.Value.LowerBound;
            lastComment = options.Value.Time;
            _httpContext = httpContextAccessor;
            filePath = address.Value.Address;
        }

        public async Task Add(ArticleRequestModel entity, Account account,string path,CancellationToken cancellationToken = default)
        {
            if (GetNumberOfPosts(account, cancellationToken).Result >= _limit)
            {
                var article = new Article
                {
                    Content = entity.Content,
                    Title = entity.Title,
                   // Path = await UploadImage(entity.Title),
                    State = Domain.Enums.State.Pending,
                    User = account,
                    Path = path
                    
                };
                await base.AddAsync(cancellationToken, article);
            }else

            throw new UserDoesNotHaveEnoughCommentsToPostArticleExpection(" with naem "+account.UserName);
        }
        public async Task<string> UploadImage(string title, CancellationToken token = default)
        {
            var uploadFile = _httpContext.HttpContext?.Request.Form.Files;
            string imagePath = "";
            if (uploadFile is not null)
            {
                foreach (var item in uploadFile)
                {
                    string filename = filePath + "\\articles\\" + title;

                    if (!Directory.Exists(filename))
                        Directory.CreateDirectory(filename);

                    imagePath = filename + $"\\{title}.png";
                    if (!File.Exists(imagePath))
                        File.Delete(imagePath);

                    using (FileStream strea = File.Create(imagePath))
                    {
                        await item.CopyToAsync(strea);
                    }
                }
            }
            return imagePath;
        }


        public async Task Delete(string ID, CancellationToken cancellationToken = default)
        {
            var target = await GetById(ID);
            target.Status = Domain.Enums.Status.Deleted;
            target.State = Domain.Enums.State.Hide;
            await base.UpdateAsync(cancellationToken, target);
        }

        public async Task<IEnumerable<Article>> GetAll(CancellationToken cancellationToken = default)
        {
            var all = await base.GetAllAsync(cancellationToken);
            return all.Where(i=>(int)i.State == 1);
        }

        public async Task<Article> GetById(string ID, CancellationToken cancellationToken = default)
        {
            return await base.GetAsync(cancellationToken, x => x.Id.ToString().ToLower().Equals(ID.ToLower()));

        }

        public async Task Update(string id, ArticleRequestModel requestModel, CancellationToken cancellationToken = default)
        {
            var entity = await GetById(id);
            if (requestModel != default)
            {
                entity.Title = requestModel.Title;
                entity.Content = requestModel.Content;
            }
            entity.ModifiedAt = DateTime.Now;
            entity.State = Domain.Enums.State.Show;
            await base.UpdateAsync(cancellationToken, entity);
        }
        public async Task<int> GetNumberOfPosts(Account account,CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(account.Articles.Count());
        }

        public async Task<IEnumerable<Article>> PendingArticles(CancellationToken cancellationToken = default)
        {
            var all = await base.GetAllAsync(cancellationToken);
            return all.Where(i => (int)i.State == 0);
        }

        public async Task DeActive(CancellationToken cancellationToken)
        {
            var articles = await base.GetAllAsync(cancellationToken);
            foreach (var item in articles)
            {
                var comments = await Comments(item.Id.ToString());
                var lastOne = comments.MaxBy(i => i.CreatedAt);
                if (lastOne != null)
                {
                    if (lastOne.CreatedAt.AddDays(lastComment) <= DateTime.Now)
                    {
                        item.Status = Domain.Enums.Status.Inactive;
                        await base.UpdateAsync(cancellationToken, item);
                    }
                }
                else
                {
                    if (item.CreatedAt.AddDays(lastComment) <= DateTime.Now)
                    {
                        item.Status = Domain.Enums.Status.Inactive;
                        await base.UpdateAsync(cancellationToken, item);
                    }
                }

            }
        }

        public async Task<IEnumerable<Comment>> Comments(string ID,CancellationToken cancellationToken = default)
        {
            var article = await GetById(ID);
            return article.Comments;
        }

        public async Task<IEnumerable<Article>> Search(string search, CancellationToken cancellationToken = default)
        {
            var all = await GetAll(cancellationToken);
            var result = all.Where(i=>i.Title.ToLower().Contains(search.ToLower()));
            return result;
        }
    }
}
