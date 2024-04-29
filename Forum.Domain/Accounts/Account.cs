using Forum.Domain.Articles;
using Forum.Domain.Comments;
using Forum.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Forum.Domain.Accounts
{
    public class Account : IdentityUser<Guid>
    {
        public bool Gender { get; set; }
        public Status Status { get; set; }
        public bool IsBanned { get; set; } = false;
        public string Path { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
