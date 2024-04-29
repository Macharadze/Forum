using Forum.Domain.Comments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forum.Persistence.Configuration
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(x => x.Status).IsRequired().IsUnicode(false);
            builder.Property(x => x.Content).IsRequired().IsUnicode(false);
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.ModifiedAt).IsRequired(false);
          //  builder.HasOne(x => x.User).WithMany(x => x.Comments).OnDelete(DeleteBehavior.NoAction);
           // builder.HasOne(x => x.Article).WithMany(x => x.Comments).OnDelete(DeleteBehavior.NoAction);
            builder.HasQueryFilter(x => (int)x.Status == 0);

        }
    }
}
