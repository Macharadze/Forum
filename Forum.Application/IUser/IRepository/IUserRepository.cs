using Forum.Application.AccountModel;
using Forum.Application.IRepository;
using Forum.Domain.Accounts;
using Forum.Domain.Articles;
using Forum.Domain.Comments;

namespace Forum.Application.IUser.IRepository
{
    public interface IUserRepository : IBaseRepository<Account>
    {
        Task Create(RegisterModel registerCancellationToken, CancellationToken cancellationToken = default);
        Task Logout(CancellationToken cancellationToken = default);
        Task AddIMage(Account account,CancellationToken cancellationToken = default);
        Task ChangePassword(Account account,string oldPassword,string newPassword,CancellationToken cancellationToken = default);
        Task DeleteIMage(Account account, CancellationToken cancellationToken = default);

        Task<Account> GetCurrentUser(CancellationToken token = default);
        Task<string> GetAuthorizedId(CancellationToken token = default);
        Task Update(AccountRequestmodel account, CancellationToken token = default);
        Task<JWTresponseModel> Login(LoginModel login, CancellationToken cancellationToken = default);
        Task<IEnumerable<Comment>> GetAllComments(string ID, CancellationToken cancellationToken = default);
        Task<IEnumerable<Article>> GetAllArticles(string email, CancellationToken cancellationToken = default);
        Task<Account> GetByEmail(string email, CancellationToken cancellationToken = default);
        Task<string> BanAccount(string email, CancellationToken cancellationToken = default);
        Task UnBanAccount(string Id, CancellationToken cancellationToken = default);
        Task<bool> ExistById(string id, CancellationToken cancellationToken = default);
        Task<bool> ExistByEmail(string email, CancellationToken cancellationToken = default);

    }
}
