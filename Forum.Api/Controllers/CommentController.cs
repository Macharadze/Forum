using Asp.Versioning;
using Forum.Api.Infrastructure.Localization;
using Forum.Api.Infrastructure.Validations;
using Forum.Application.CommentModel;
using Forum.Application.Exceptions;
using Forum.Application.IComment;
using Forum.Domain.Articles;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Api.Controllers
{
    /// <summary>
    /// Controller for managing User Comments.
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion(1.0)]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        /// <summary>
        /// Constructor for AccountController.
        /// </summary>
        /// <param name="commentService">The Comments service dependency.</param>
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }
        /// <summary>
        /// Creates a new comment for an article.
        /// </summary>
        /// <param name="request">The comment request model containing comment details.</param>
        /// <param name="articleID">The ID of the article to which the comment belongs.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>
        /// 
        /// Status codes:
        /// 200 OK - If the comment is created successfully.
        /// 400 Bad Request - If the comment is not allowed.
        /// </returns>
        [HttpPost("create"),Authorize]
        public async Task<ActionResult> CreateComment([FromBody] CommentRequestModel request, string articleID, CancellationToken cancellationToken)
        {
            try
            {
                var validation = new CommentValidation();
                var result = await validation.ValidateAsync(request);
                if (!result.IsValid)
                {
                    return BadRequest(result.Errors);
                }
                await _commentService.CreateAsync(request, articleID, cancellationToken);
                return Ok(Language.Create);
            }
            catch (CommentIsNotAllowedException ex)
            {
                return BadRequest(Language.CommentIsNotAllowed);
            }
        }
        /// <summary>
        /// Deletes a comment by its ID.
        /// </summary>
        /// <param name="id">The ID of the comment to delete.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>
        /// 
        /// Status codes:
        /// 200 OK - If the comment is deleted successfully.
        /// </returns>
        [HttpDelete("delete/{id}"),Authorize]
        public async Task<ActionResult> DeleteComment(string id, CancellationToken cancellationToken)
        {
          
                await _commentService.DeleteAsync(id, cancellationToken);
                return Ok(Language.Delete);
         
        }
        /// <summary>
        /// Retrieves a comment by its ID.
        /// </summary>
        /// <param name="id">The ID of the comment to retrieve.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>
        /// 
        /// Status codes:
        /// 200 OK - If the comment is found and retrieved successfully.
        /// 404 Not Found - If the comment does not exist.
        /// </returns>
        [HttpGet("get/{id}"),Authorize]
        public async Task<ActionResult<CommentRequestModel>> GetCommentById(string id, CancellationToken cancellationToken)
        {
           
                var comment = await _commentService.GetByIdAsync(id, cancellationToken);
                if (comment == null)
                    return NotFound(Language.Conflict);

                var result = comment.Adapt<CommentRequestModel>();

                return Ok(result);
          
        }
    }
}
