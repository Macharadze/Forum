using Forum.Application.CommentModel;

namespace Forum.Application.ArticleModel
{
    public class ArticleResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<CommentResponseModel> Comments { get; set; }
    }
}
