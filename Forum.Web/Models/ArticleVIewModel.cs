using Forum.Application.ArticleModel;

namespace Forum.Web.Models
{
    public class ArticleViewModel
    {
        public int CurrentPageIndex { get; set; }
        public int TotalPages { get; set; }
        public List<ArticleResponse> Articles { get; set; }

    }
}
