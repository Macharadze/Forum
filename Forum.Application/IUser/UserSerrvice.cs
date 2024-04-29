using Forum.Application.AccountModel;
using Forum.Application.Exceptions;
using Forum.Application.UOW;
using Forum.Domain.Accounts;
using Forum.Domain.Articles;
using Forum.Domain.Bans;
using Forum.Domain.Comments;

namespace Forum.Application.IUser
{
    public class UserSerrvice : IUserService
    {
        private readonly IUserUOW _ouw;
        public UserSerrvice(IUserUOW ouw)
        {
            _ouw = ouw;
        }

        public async Task BanAccount(string email, int duration, CancellationToken cancellationToken = default)
        {

            var targetID = await _ouw._userRepository.BanAccount(email);
            if (targetID is null)
            {
                throw new UserDoesnotExistException("");
            }

            var ban = new AccountBan
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMinutes(duration),
                AccountID = targetID
            };
            await _ouw._banAccountRepository.Add(ban);

        }
        public async Task UnBanAccount(CancellationToken cancellationToken = default)
        {
            try
            {
                var BannAccounts = await _ouw._banAccountRepository.GetAll(cancellationToken);
                foreach (var ban in BannAccounts)
                {
                    if (await _ouw._banAccountRepository.CheckIfExpired(ban.EndDate))
                        await _ouw._userRepository.UnBanAccount(ban.AccountID, cancellationToken);
                }
            }
            catch (UserDoesnotExistException)
            {
                throw new UserDoesnotExistException("");
            }

        }
        public async Task Create(RegisterModel register, CancellationToken cancellationToken = default)
        {

            if (await _ouw._userRepository.ExistByEmail(register.Email))
            {
                throw new UserAlreadyExistException("");
            }
            await _ouw._userRepository.Create(register, cancellationToken);


        }

        public async Task<JWTresponseModel> Login(LoginModel login, CancellationToken cancellationToken = default)
        {

            return await _ouw._userRepository.Login(login, cancellationToken);

        }
        public async Task Delete(string id, CancellationToken cancellationToken = default)
        {

            if (!await _ouw._userRepository.ExistById(id))
            {
                throw new UserDoesnotExistException("");
            }
            await _ouw._userRepository.Delete(id);

        }

        public async Task DeleteIMage(string id, CancellationToken cancellationToken = default)
        {

            if (!await _ouw._userRepository.ExistById(id))
            {
                throw new UserDoesnotExistException("");
            }
            var account = await _ouw._userRepository.GetById(id, cancellationToken);
            await _ouw._userRepository.DeleteIMage(account);


        }

        public async Task<IEnumerable<Account>> GetAll(CancellationToken cancellationToken = default)
        {

            return await _ouw._userRepository.GetAll(cancellationToken);

        }

        public async Task<Account> GetById(string id, CancellationToken cancellationToken = default)
        {

            if (!await _ouw._userRepository.ExistById(id))
            {
                throw new UserDoesnotExistException("");
            }
            return await _ouw._userRepository.GetById(id, cancellationToken);


        }

        public async Task Update(AccountRequestmodel user, CancellationToken cancellationToken = default)
        {


            await _ouw._userRepository.Update(user, cancellationToken);

        }

        public async Task UploadImage(string email, CancellationToken cancellationToken = default)
        {
            if (!await _ouw._userRepository.ExistByEmail(email))
            {
                throw new UserDoesnotExistException("");
            }


            var account = await _ouw._userRepository.GetByEmail(email, cancellationToken);
            await _ouw._userRepository.AddIMage(account, cancellationToken);

        }

        public async Task<Account> GetByEmail(string email, CancellationToken cancellationToken = default)
        {

            if (!await _ouw._userRepository.ExistByEmail(email))
            {
                throw new UserDoesnotExistException("");
            }
            return await _ouw._userRepository.GetByEmail(email);


        }

        public async Task<IEnumerable<Comment>> GetAllComments(string ID, CancellationToken cancellationToken = default)
        {

            if (!await _ouw._userRepository.ExistById(ID))
            {
                throw new UserDoesnotExistException("");
            }
            return await _ouw._userRepository.GetAllComments(ID, cancellationToken);



        }

        public async Task<IEnumerable<Article>> GetAllArticles(string email, CancellationToken cancellationToken = default)
        {

            if (!await _ouw._userRepository.ExistByEmail(email))
            {
                throw new UserDoesnotExistException("");
            }
            return await _ouw._userRepository.GetAllArticles(email, cancellationToken);


        }

        public async Task Logout(CancellationToken cancellationToken = default)
        {
            await _ouw._userRepository.Logout(cancellationToken);
        }

        public async Task<Account> GetCurrentUser(CancellationToken token = default)
        {
            var owner = await _ouw._userRepository.GetCurrentUser(token);
            return owner;
        }

        public async Task ChangePassword(string oldPassword, string newPassword, string confirmPassword, CancellationToken cancellationToken = default)
        {
            if (!newPassword.Equals(confirmPassword))
            {
                throw new ConfirmedPasswordIsNotCorrect("");
            }
            var user = await GetCurrentUser(cancellationToken);
            await _ouw._userRepository.ChangePassword(user, oldPassword, newPassword, cancellationToken);
        }
    }
}
