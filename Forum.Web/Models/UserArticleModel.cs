using Forum.Application.ArticleModel;

namespace Forum.Web.Models
{
    public class UserArticleModel
    {
        public bool IsOwner { get; set; }
        public List<ArticleResponse> Articles { get; set; }
    }
}
