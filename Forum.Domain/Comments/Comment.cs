using Forum.Domain.Accounts;
using Forum.Domain.Articles;
using Forum.Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace Forum.Domain.Comments
{
    public class Comment : BaseClass
    {
        public string Content { get; set; }

        public virtual Account User { get; set; }

        public virtual Article Article { get; set; }
    }
}
