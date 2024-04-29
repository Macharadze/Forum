using Forum.Application.AccountModel;
using Forum.Domain.Accounts;
using Forum.Domain.Articles;
using Forum.Domain.Comments;
using Microsoft.Extensions.DependencyInjection;

namespace Forum.Application.IUser
{
    public interface IUserService
    {
        Task<Account> GetById(string id, CancellationToken cancellationToken = default);
        Task Logout(CancellationToken cancellationToken = default);
        Task<Account> GetByEmail(string email, CancellationToken cancellationToken = default);
        Task Update(AccountRequestmodel user, CancellationToken cancellationToken = default);
        Task Delete(string Id, CancellationToken cancellationToken = default);
        Task Create(RegisterModel registerCancellationToken, CancellationToken cancellationToken = default);
        Task<JWTresponseModel> Login(LoginModel login, CancellationToken cancellationToken = default);
        Task BanAccount(string email, int duration, CancellationToken cancellationToken = default);
        Task UnBanAccount(CancellationToken cancellationToken = default);
        Task UploadImage(string path, CancellationToken cancellationToken = default);
        Task DeleteIMage(string id,CancellationToken cancellationToken = default);
        Task ChangePassword(string oldPassword, string newPassword,string confirmPassword, CancellationToken cancellationToken = default);

        Task<Account> GetCurrentUser(CancellationToken token = default);

        Task<IEnumerable<Comment>> GetAllComments(string ID, CancellationToken cancellationToken = default);
        Task<IEnumerable<Article>> GetAllArticles(string email, CancellationToken cancellationToken = default);
        Task<IEnumerable<Account>> GetAll(CancellationToken cancellationToken = default);

    }
 
}
