using Forum.Application.ArticleModel;
using Forum.Application.UOW;
using Forum.Domain.Accounts;
using Forum.Domain.Articles;
using Forum.Domain.Comments;

namespace Forum.Application.IArticle
{
    public class ArticleService : IArticleService
    {


        private readonly IArticleUOW _uow;

        public ArticleService(IArticleUOW uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<Comment>> Comments(string ID, CancellationToken cancellationToken = default)
        {
           
                return await _uow._articleRepository.Comments(ID, cancellationToken);
          
        }

        public async Task Create(CancellationToken cancellationToken, ArticleRequestModel article)
        {
           
                var owner = await Owner(cancellationToken);
                var path = await _uow._articleRepository.UploadImage(article.Title);
                await _uow._articleRepository.Add(article, owner, path,cancellationToken);
          
        }

        public async Task DeActive(CancellationToken cancellationToken)
        {
         
                await _uow._articleRepository.DeActive(cancellationToken);
           
        }

        public async Task Delete(CancellationToken cancellationToken, string id)
        {
           
                await _uow._articleRepository.Delete(id);
           
        }

        public async Task<IEnumerable<Article>> GetAll(CancellationToken cancellationToken)
        {
           
                return await _uow._articleRepository.GetAll(cancellationToken);
          
        }

        public async Task<IEnumerable<Article>> GetAllArticlesByEmail(string email, CancellationToken cancellationToken = default)
        {
            return await _uow._userRepository.GetAllArticles(email, cancellationToken);
        }

        public async Task<Article> GetArticle(CancellationToken cancellationToken, string id)
        {
           
                var article = await _uow._articleRepository.GetById(id, cancellationToken);
                return await _uow._articleRepository.GetById(id, cancellationToken);
            
        }

 
        public async Task<Account> Owner(CancellationToken cancellation)
        {
            return await _uow._userRepository.GetCurrentUser(cancellation);
        }

        public async Task<IEnumerable<Article>> PendingArticle(CancellationToken cancellationToken)
        {
            
                return await _uow._articleRepository.PendingArticles(cancellationToken);
           
        }

        public async Task<IEnumerable<Article>> Search(string search, CancellationToken cancellationToken = default)
        {
            return await _uow._articleRepository.Search(search, cancellationToken);
        }

        public async Task Update(CancellationToken cancellationToken, string id, ArticleRequestModel requestModel)
        {
          
                await _uow._articleRepository.Update(id,requestModel ,cancellationToken);
           
        }
    }
}
