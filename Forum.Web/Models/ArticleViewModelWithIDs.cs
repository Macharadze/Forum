using Forum.Application.CommentModel;

namespace Forum.Web.Models
{
    public class ArticleViewModelWithIDs
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Path { get; set; }
        public string Content { get; set; }
        public List<CommentViewModel> Comments { get; set; }
    }
}
