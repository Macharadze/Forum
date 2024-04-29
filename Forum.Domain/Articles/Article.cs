using Forum.Domain.Accounts;
using Forum.Domain.Base;
using Forum.Domain.Comments;
using Forum.Domain.Enums;

namespace Forum.Domain.Articles
{
    public class Article : BaseClass
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public State State { get; set; }

        public virtual Account User { get; set; }

        public string Path { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
