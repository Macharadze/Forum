using Forum.Application.AccountModel;
using Forum.Application.ArticleModel;
using Forum.Application.Exceptions;
using Forum.Application.IArticle;
using Forum.Domain.Roles;
using Forum.Persistence.ConfigurationsAppsettingJson;
using Forum.Web.Models;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Forum.Web.Controllers
{

    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private int _maxRow;
        public ArticleController(IArticleService articleService, IOptions<LimitConfiguration> options)
        {
            _articleService = articleService;
            _maxRow = options.Value.MaxRowPerPage;
        }

        [HttpGet]
        public async Task<ActionResult<List<ArticleResponse>>> UserArticles(string email, bool IsOwner ,CancellationToken token)
        {
            var userArticles = new UserArticleModel();
            var articles = await _articleService.GetAllArticlesByEmail(email, token); ;
            userArticles.IsOwner = IsOwner;
            userArticles.Articles = articles.Adapt<List<ArticleResponse>>();
            return View(await Task.FromResult(userArticles));
        }
        [HttpGet]
        public async Task<ActionResult<ArticleViewModel>> Articles(CancellationToken token)
        {

            var articles = await _articleService.GetAll(token);
            var result = await GetArticleList(1, articles.Adapt<List<ArticleResponse>>());
            return View(result);

        }
        [HttpPost]
        public async Task<ActionResult<ArticleViewModel>> Articles(string email, string search, int currentPage, CancellationToken token)
        {


            var articles = !string.IsNullOrWhiteSpace(search) ? await _articleService.Search(search, token) : await _articleService.GetAll(token);
            var result = await GetArticleList(currentPage, articles.Adapt<List<ArticleResponse>>());

            return View(result);

        }

        [HttpGet]
        public async Task<ActionResult> Article(string id, CancellationToken token)
        {

            var article = await _articleService.GetArticle(token, id);
            var resutl = article.Adapt<ArticleViewModelWithIDs>();

            return View(resutl);

        }
        public IActionResult Create()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ArticleRequestModel requestModel, CancellationToken token)

        {
            try
            {
                await _articleService.Create(token, requestModel);
                return RedirectToAction("Articles", "Article");
            }
            catch (UserDoesNotHaveEnoughCommentsToPostArticleExpection ex)
            {
                TempData["ErrorMessage"] = "You do not have enough comments to post an article.";
                return RedirectToAction("Create", "Article");
            }
        }
        [Authorize(Roles = $"{Roles.Admin}")]
        [HttpGet]
        public async Task<IActionResult> Pending(CancellationToken token)
        {
            var articles = await _articleService.PendingArticle(token);
            var toResponsemodels = articles.Adapt<List<ArticleResponse>>();
            return View(toResponsemodels.ToList());

        }
        public IActionResult Accept()
        {
            return View();
        }
        [Authorize(Roles = $"{Roles.Admin}")]
        [HttpPost]
        public async Task<IActionResult> Accept(string id, CancellationToken token)
        {
            await _articleService.Update(token,id,default!);
            
            return RedirectToAction("Pending", "Article");

        }
      
        [Authorize(Roles = $"{Roles.Admin},{Roles.Customer}")]
        [HttpPost]
        public async Task<ActionResult> Delete([FromForm] string id, bool isPending, CancellationToken token)
        {
            await _articleService.Delete(token, id);
            if (isPending)
                return RedirectToAction("Pending", "Article");

            return RedirectToAction("Articles", "Article");
        }
        private async Task<ArticleViewModel> GetArticleList(int currentPage, List<ArticleResponse> articleResponses)
        {
            var articleModel = new ArticleViewModel();


            articleModel.Articles = articleResponses.OrderBy(x => x.Title)
                .Skip((currentPage - 1) * _maxRow)
                .Take(_maxRow).ToList();

            double pageCount = (double)((decimal)articleResponses.Count() / Convert.ToDecimal(_maxRow));

            articleModel.TotalPages = (int)Math.Ceiling(pageCount);
            articleModel.CurrentPageIndex = currentPage;
            return await Task.FromResult(articleModel);
        }
        public IActionResult Update()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Update(string id,[FromForm] ArticleRequestModel article, CancellationToken token)
        {
            
            try
            {
                await _articleService.Update(token,id,article);
                return View();
            }
            catch (UserDoesnotExistException ex)
            {
                TempData["ErrorMessage"] = "User does not exists";
                return View();
            }

        }

    }

}
