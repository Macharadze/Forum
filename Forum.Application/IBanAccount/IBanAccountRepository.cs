using Forum.Domain.Bans;
using System.Globalization;

namespace Forum.Application.IBanAccount
{
    public interface IBanAccountRepository
    {
        Task<List<AccountBan>> GetAll(CancellationToken cancellationToken = default);
        Task Add(AccountBan ban, CancellationToken cancellationToken = default);
        Task<bool> CheckIfExpired(DateTime EndTime, CancellationToken cancellationToken = default);
    }
}
