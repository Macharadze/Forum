using Forum.Application.IUser.IRepository;

namespace Forum.Application.UOW
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository _userRepository { get; }
      /*  void SaveChanges();
        Task SaveChangesAsync(CancellationToken token);*/
    }
}
