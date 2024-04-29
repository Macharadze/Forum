using Forum.Application.AccountModel;
using Forum.Application.Exceptions;
using Forum.Application.IUser.IRepository;
using Forum.Domain.Accounts;
using Forum.Domain.Articles;
using Forum.Domain.Comments;
using Forum.Infrastructure.Base;
using Forum.Persistence.ConfigurationsAppsettingJson;
using Forum.Persistence.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Forum.Infrastructure.Accounts
{
    public class AccounrRepository : BaseRepository<Account>, IUserRepository
    {
        private readonly UserManager<Account> _userManager;
        private readonly SignInManager<Account> _signInManager;
        private readonly IHttpContextAccessor _httpContext;
        private readonly  string filePath;

        public AccounrRepository(DataContext context, UserManager<Account> userManager, IHttpContextAccessor httpContext, SignInManager<Account> signInManager,IOptions<ImagesAddress> options) : base(context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContext = httpContext;
            filePath = options.Value.Address;
        }

        public async Task<JWTresponseModel> Login(LoginModel login, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByNameAsync(login.UserName);
            if(user is null)
                throw new UserDoesnotExistException("with username " + login.UserName);

            if (user.IsBanned)
                    throw new UserIsBannedException("with username " + login.UserName);

            var result = await _signInManager.PasswordSignInAsync(user.UserName, login.Password, false, false);
            if (result.Succeeded)
            {
                var role = await _userManager.GetRolesAsync(user);
                var nameClaim = new Claim(ClaimTypes.Name, user.UserName);
                var identifierClaim = new Claim(ClaimTypes.NameIdentifier, user.Id.ToString());
                var authClaims = new List<Claim>
                {
                    nameClaim, identifierClaim
                };
                await _userManager.AddClaimAsync(user, nameClaim);
                await _userManager.AddClaimAsync(user, identifierClaim);
               
                foreach (var userRole in role)
                {
                    var claim = new Claim(ClaimTypes.Role, userRole);
                    authClaims.Add(claim);
                    await _userManager.AddClaimAsync(user, claim);

                }
                return new JWTresponseModel { Claims = authClaims };
            }
            throw new PasswordIsNotCorrect("");
        }
        public async Task Create(RegisterModel register, CancellationToken cancellationToken = default)
        {
            var account = new Account
            {
                UserName = register.Username,
                Email = register.Email,
                Gender = register.Gender,
                PhoneNumber = register.Phone,
                Status = Domain.Enums.Status.Active

            };
            await _userManager.CreateAsync(account, register.Password);

            await _userManager.AddToRoleAsync(account, "Customer");
            await base._context.SaveChangesAsync();
        }

        public async Task Delete(string ID, CancellationToken cancellationToken = default)
        {
            var target = await GetById(ID);
            target.Status = Domain.Enums.Status.Deleted;

            await DeleteArticles(target.Articles.ToList(), cancellationToken);
            await DeleteComment(target.Comments.ToList(), cancellationToken);

            await Update(target, cancellationToken);
        }

        private async Task DeleteArticles(List<Article> articles, CancellationToken token)
        {
            foreach (var article in articles)
                article.Status = Domain.Enums.Status.Deleted;

            await Task.CompletedTask;
        }
        private async Task DeleteComment(List<Comment> comments, CancellationToken token)
        {
            foreach (var comment in comments)
                comment.Status = Domain.Enums.Status.Deleted;

            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Article>> GetAllArticles(string email, CancellationToken cancellationToken = default)
        {
            var target = await GetByEmail(email);
            return target.Articles;
        }

        public async Task<IEnumerable<Account>> GetAll(CancellationToken cancellationToken = default)
        {
            var all = await base.GetAllAsync(cancellationToken);
            return all.Where(i => !i.IsBanned);
        }

        public async Task<IEnumerable<Comment>> GetAllComments(string ID, CancellationToken cancellationToken = default)
        {
            var target = await GetById(ID);
            return target.Comments;
        }

        public async Task<Account> GetByEmail(string email, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user;
        }

        public async Task<Account> GetById(string ID, CancellationToken cancellationToken = default)
        {
            return await base.GetAsync(cancellationToken, x => x.Id.ToString().ToLower().Equals(ID.ToLower()));
        }
        public async Task Update(Account entity, CancellationToken cancellationToken = default)
        {
            await base.UpdateAsync(cancellationToken, entity);
        }

        public async Task<string> BanAccount(string email, CancellationToken cancellationToken = default)
        {
            var target = await _userManager.FindByEmailAsync(email);

            target.IsBanned = true;
           // await _signInManager.SignOutAsync();
            await base.UpdateAsync(cancellationToken, target);
            return target.Id.ToString();
        }

        public async Task UnBanAccount(string Id ,CancellationToken cancellationToken = default)
        {
            var target = await GetById(Id, cancellationToken);

            target.IsBanned = false;

            await base.UpdateAsync(cancellationToken, target);
        }
        public async Task<Account> GetCurrentUser(CancellationToken token = default)
        {
            var id = await GetAuthorizedId();
            var user = await GetById(id, token);
            return user;
        }
        public async Task<string> GetAuthorizedId(CancellationToken token = default)
        {
            var http = _httpContext.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            if (http is null)
                throw new InvalidOperationException("");

            return await Task.FromResult(http.Value.ToString());
        }

        public async Task AddIMage(Account account,CancellationToken cancellationToken = default)
        {
            var path = await UploadImage(account.Email,cancellationToken);
            account.Path = path;
            await base.UpdateAsync(cancellationToken, account);
        }
        public async Task<string> UploadImage(string Email,CancellationToken token = default)
        {
            var uploadFile = _httpContext.HttpContext?.Request.Form.Files;
            string imagePath = "";
            if(uploadFile is not null) {
                foreach (var item in uploadFile)
                {
                    string filename = filePath + "\\Accounts\\" + Email;

                    if (!Directory.Exists(filename))
                        Directory.CreateDirectory(filename);

                    imagePath = filename + $"\\{Email}.png";
                    if (!File.Exists(imagePath))
                        File.Delete(imagePath);

                    using (FileStream strea = File.Create(imagePath))
                    {
                        await item.CopyToAsync(strea);
                    }
                }
            }
            return imagePath; 
        }

        public async Task DeleteIMage(Account account,CancellationToken cancellationToken = default)
        {
            if (!File.Exists(account.Path))
                File.Delete(account.Path);

            account.Path = "";
            
            await base.UpdateAsync(cancellationToken, account);
        }

        public async Task Update(AccountRequestmodel account, CancellationToken token = default)
        {
            var user = await GetCurrentUser();
            user.Email = account.Email;
            user.PhoneNumber = account.PhoneNumber;
            user.UserName = account.UserName;
            await _userManager.UpdateAsync(user);
            await base.UpdateAsync(token,user);

        }

        public async Task<bool> ExistById(string id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(i => i.Id.ToString().ToLower().Equals(id.ToLower())&& !i.IsBanned );
        }

        public async Task<bool> ExistByEmail(string email, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(i => i.Email.ToLower().Equals(email.ToLower())&& !i.IsBanned);

        }

        public async Task Logout(CancellationToken cancellationToken = default)
        {
            await _signInManager.SignOutAsync();
        }

        public  async Task ChangePassword(Account account, string oldPassword, string newPassword, CancellationToken cancellationToken = default)
        {
             await _userManager.ChangePasswordAsync(account,oldPassword,newPassword);
        }
    }
}
