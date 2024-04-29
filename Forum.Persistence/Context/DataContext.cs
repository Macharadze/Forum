using Forum.Domain.Accounts;
using Forum.Domain.Articles;
using Forum.Domain.Bans;
using Forum.Domain.Comments;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Forum.Persistence.Context
{
    public class DataContext : IdentityDbContext<Account,IdentityRole<Guid>,Guid>
    {
        
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<AccountBan> Bans { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseLazyLoadingProxies();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
            base.OnModelCreating(builder);
        }
    }
}
