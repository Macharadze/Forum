using Forum.Application.IBanAccount;

namespace Forum.Application.UOW
{
    public interface IUserUOW : IUnitOfWork
    {
        IBanAccountRepository _banAccountRepository { get; }
    }
}
