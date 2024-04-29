using Microsoft.AspNetCore.Http;

namespace Forum.Application.ArticleModel
{
    public class ArticleRequestModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public IFormFile file { get; set; } 
    }
}
