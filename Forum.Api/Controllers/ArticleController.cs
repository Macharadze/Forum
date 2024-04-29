using Asp.Versioning;
using Forum.Api.Infrastructure.Localization;
using Forum.Api.Infrastructure.Validations;
using Forum.Application.ArticleModel;
using Forum.Application.CommentModel;
using Forum.Application.Exceptions;
using Forum.Application.IArticle;
using Forum.Domain.Articles;
using Forum.Domain.Roles;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Api.Controllers
{
    /// <summary>
    /// Controller for managing user Articles.
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion(1.0)]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
        /// <summary>
        /// Constructor for AccountController.
        /// </summary>
        /// <param name="articleService">The article service dependency.</param>
        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }
        /// <summary>
        /// Retrieves comments for a specific article.
        /// </summary>
        /// <param name="articleId">The ID of the article for which comments are to be retrieved.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>
        /// 
        /// Status codes:
        /// 200 OK - If comments are retrieved successfully.
        /// </returns>
        [AllowAnonymous]
        [HttpGet("Comments")]
        public async Task<ActionResult<List<CommentRequestModel>>> GetComments(string articleId, CancellationToken cancellationToken)
        {
           
                var comments = await _articleService.Comments(articleId, cancellationToken);
                var result = comments.Adapt<List<CommentRequestModel>>();
                return Ok(result);
            
         
        }
        /// <summary>
        /// Creates a new article.
        /// </summary>
        /// <param name="article">The article request model containing article details.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>
        /// 
        /// Status codes:
        /// 200 OK - If the article is created successfully.
        /// 400 Bad Request - If the user does not have enough comments to post an article.
        /// </returns>

        [HttpPost("Create"),Authorize(Roles = $"{Roles.Customer},{Roles.Admin}")]
        public async Task<ActionResult> CreateArticle([FromBody] ArticleRequestModel article, CancellationToken cancellationToken)
        {
            try
            {
                var validation = new ArticleValidation();
                var result = await validation.ValidateAsync(article);
                if (!result.IsValid)
                {
                    return BadRequest(result.Errors);
                }
                await _articleService.Create(cancellationToken, article);
                return Ok(Language.Create);
            }
            catch (UserDoesNotHaveEnoughCommentsToPostArticleExpection ex)
            {
                return BadRequest(Language.UserIsNotAllowedToCreatePost);
            }
        }

        /// <summary>
        /// Deletes an article by its ID.
        /// </summary>
        /// <param name="id">The ID of the article to delete.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>
        /// 
        /// Status codes:
        /// 200 OK - If the article is deleted successfully.
        /// </returns>
        [HttpDelete("Delete/{id}"), Authorize(Roles = $"{Roles.Customer},{Roles.Admin}")]
        public async Task<ActionResult> DeleteArticle(string id, CancellationToken cancellationToken)
        {
           
                await _articleService.Delete(cancellationToken, id);
                return Ok(Language.Delete);
           
        }
        /// <summary>
        /// Retrieves all articles.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>
        /// 
        /// Status codes:
        /// 200 OK - If articles are retrieved successfully.
        /// </returns>
        [AllowAnonymous]
        [HttpGet("Articles")]
        public async Task<ActionResult<List<ArticleResponse>>> GetAllArticles(CancellationToken cancellationToken)
        {
                var articles = await _articleService.GetAll(cancellationToken);
                var result = articles.Adapt<List<ArticleResponse>>();
                return Ok(result);

        }
        /// <summary>
        /// Retrieves an article by its ID.
        /// </summary>
        /// <param name="id">The ID of the article to retrieve.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>
        /// 
        /// Status codes:
        /// 200 OK - If the article is found and retrieved successfully.
        /// </returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ArticleResponse>> GetArticle(string id, CancellationToken cancellationToken)
        {
          
                var article = await _articleService.GetArticle(cancellationToken, id);
                var result = article.Adapt<ArticleResponse>();
                return Ok(result);
           
        }
        /// <summary>
        /// Retrieves pending articles.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>
        /// 
        /// Status codes:
        /// 200 OK - If pending articles are retrieved successfully.
        /// </returns>
        [HttpGet("Pending"), Authorize(Roles = $"{Roles.Admin}")]
        public async Task<ActionResult<List<ArticleResponse>>> GetPendingArticles(CancellationToken cancellationToken)
        {
           
                var pendingArticles = await _articleService.PendingArticle(cancellationToken);
                var result = pendingArticles.ToList().Adapt<List<ArticleResponse>>();
                return Ok(result); 
           
        }

        /// <summary>
        /// accept an article by its ID.
        /// </summary>
        /// <param name="id">The ID of the article to accept.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>
        /// 
        /// Status codes:
        /// 200 OK - If the article is updated successfully.
        /// </returns>
        [HttpPut("Accept/{id}"), Authorize(Roles = $"{Roles.Admin}")]
        public async Task<ActionResult> Accept(string id, CancellationToken cancellationToken)
        {

            await _articleService.Update(cancellationToken, id, default!);
            return Ok("Article updated successfully.");

        } /// <summary>
          /// Updates an article by its ID.
          /// </summary>
          /// <param name="id">The ID of the article to update.</param>
          /// <param name="articleRequestModel">The model of article  request to update.</param>
          /// <param name="cancellationToken">Cancellation token for the operation.</param>
          /// <returns>
          /// 
          /// Status codes:
          /// 200 OK - If the article is updated successfully.
          /// </returns>
        [HttpPut("Update/{id}"), Authorize]
        public async Task<ActionResult> Update(string id,ArticleRequestModel articleRequestModel, CancellationToken cancellationToken)
        {

            await _articleService.Update(cancellationToken, id, articleRequestModel);
            return Ok("Article updated successfully.");

        }
    }

}

