using Forum.Application.IBanAccount;
using Forum.Domain.Bans;
using Forum.Infrastructure.Base;
using Forum.Persistence.Context;

namespace Forum.Infrastructure.BanAccounts
{
    public class BanAccountRepository : BaseRepository<AccountBan>, IBanAccountRepository
    {
        public BanAccountRepository(DataContext context) : base(context)
        {
        }

        public async Task Add(AccountBan ban, CancellationToken cancellationToken = default)
        {
            await base.AddAsync(cancellationToken, ban);
        }

        public async Task<bool> CheckIfExpired(DateTime EndDate, CancellationToken cancellationToken = default)
        {
            if(EndDate <= DateTime.Now)
                return await Task.FromResult(true);
            return await Task.FromResult(false);
        }

        public async Task<List<AccountBan>> GetAll( CancellationToken cancellationToken = default)
        { 
            return await base.GetAllAsync(cancellationToken);
        }
    }
}
