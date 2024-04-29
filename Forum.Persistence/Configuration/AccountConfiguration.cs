using Forum.Domain.Accounts;
using Forum.Domain.Articles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forum.Persistence.Configuration
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.Gender).IsRequired();
            builder.Property(x => x.Email).IsRequired();
            builder.Property(x => x.Path).IsRequired(false);
            builder.HasMany(x => x.Articles).WithOne(x=> x.User).OnDelete(DeleteBehavior.NoAction); ;
            builder.HasMany(x => x.Comments).WithOne(x=>x.User).OnDelete(DeleteBehavior.NoAction); ;

            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasQueryFilter(x => (int)x.Status == 0);
        //    builder.HasQueryFilter(x => !x.IsBanned);
            builder.Ignore(x => x.TwoFactorEnabled);
            builder.Ignore(x => x.EmailConfirmed);
        }
    }
}
