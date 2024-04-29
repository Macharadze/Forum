using Forum.Domain.Bans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forum.Persistence.Configuration
{
    public class BanConfiguration : IEntityTypeConfiguration<AccountBan>
    {
        public void Configure(EntityTypeBuilder<AccountBan> builder)
        {
            builder.HasKey(x => x.Id);
            
        }
    }
}
