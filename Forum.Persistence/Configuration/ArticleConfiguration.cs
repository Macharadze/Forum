using Forum.Domain.Articles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forum.Persistence.Configuration
{
    public class ArticleConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(x=>x.Status).IsRequired().IsUnicode(false);
            builder.Property(x=>x.Title).IsRequired().IsUnicode(false);
            builder.Property(x=>x.CreatedAt).IsRequired();
            builder.Property(x => x.ModifiedAt).IsRequired(false);

            builder.Property(x => x.Path).IsRequired(false);
            builder.HasMany(x => x.Comments).WithOne(x => x.Article)
                .OnDelete(DeleteBehavior.NoAction); 
         //   builder.HasOne(x => x.User).WithMany(x => x.Articles).OnDelete(DeleteBehavior.Restrict);
            builder.HasQueryFilter(x => (int)x.Status != 2);
            builder.HasQueryFilter(x => (int)x.State != 2);
           
        }
    }
}
