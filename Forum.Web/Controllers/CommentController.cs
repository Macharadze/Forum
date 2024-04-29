using Forum.Application.CommentModel;
using Forum.Application.Exceptions;
using Forum.Application.IComment;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(string content, string Id, CancellationToken token)
        {
            try { 
            var requestModel = new CommentRequestModel { Content = content };
            await _commentService.CreateAsync(requestModel, Id, token);
            return RedirectToAction("Article", "Article", new { id = Id });
            }
            catch (CommentIsNotAllowedException ex)
            {
                TempData["ErrorMessage"] = "Comment is not allowed";

                return RedirectToAction("Article", "Article", new { id = Id });
            }


        }
        public ActionResult Delete()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Delete(string commentID, string postID, CancellationToken token)
        {

            await _commentService.DeleteAsync(commentID, token);

            return RedirectToAction("Article", "Article", new { id = postID });

        }



    }
}
