using Forum.Application.ArticleModel;
using Forum.Application.CommentModel;

namespace Forum.Application.AccountModel
{
    public class AccountResponseModel
    {
        public string UserName { get; set; }
        public string Path { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<ArticleResponse> ArticleResponses { get; set; }
    }
}
