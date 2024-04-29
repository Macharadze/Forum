using Forum.Application.IBanAccount;
using Forum.Application.IUser.IRepository;
using Forum.Application.UOW;
using Forum.Persistence.Context;

namespace Forum.Infrastructure.UnitOfWorks
{
    public class AccountUOW : UnitOFWork, IUserUOW
    {
        public IBanAccountRepository _banAccountRepository { get; }

        public IUserRepository _userRepository { get; }

        public AccountUOW(DataContext dbContext,IBanAccountRepository banAccount, IUserRepository userRepository) : base(dbContext)
        {
            _banAccountRepository = banAccount;
            _userRepository = userRepository;
        }

    }
}
