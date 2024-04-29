using Asp.Versioning;
using Forum.Api.Infrastructure.Auth;
using Forum.Api.Infrastructure.Localization;
using Forum.Api.Infrastructure.Validations;
using Forum.Api.Models;
using Forum.Application.AccountModel;
using Forum.Application.ArticleModel;
using Forum.Application.CommentModel;
using Forum.Application.Exceptions;
using Forum.Application.IUser;
using Forum.Domain.Roles;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading;

namespace Forum.Api.Controllers
{
    /// <summary>
    /// Controller for managing user accounts.
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion(1.0)]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IOptions<JWTConfiguration> _options;
        /// <summary>
        /// Constructor for AccountController.
        /// </summary>
        /// <param name="userService">The user service dependency.</param>
        /// <param name="options">The JWT configuration options.</param>
        public AccountController(IUserService userService, IOptions<JWTConfiguration> options)
        {
            _userService = userService;
            _options = options;
        }
        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="user">The registration model containing user details.</param>
        /// <param name="cancellation">Cancellation token for the operation.</param>
        /// <returns>
        /// 
        /// Status codes:
        /// 200 OK - If the registration is successful.
        /// 409 Conflict - If the user already exists.
        /// </returns>
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<ActionResult> Register(RegisterModel user, CancellationToken cancellation = default)
        {
            try
            {
                var validation = new UserVlidation();
                var result = await validation.ValidateAsync(user);
                if (!result.IsValid)
                {
                    return BadRequest(result.Errors);
                }
                await _userService.Create(user, cancellation);

                return Ok(Language.Create);
            }
            catch (UserAlreadyExistException EX)
            {
                return Conflict(Language.Conflict);
            }

        }
        /// <summary>
        /// Logs out the current user.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for asynchronous operation
        [HttpPost("logout"), Authorize(Roles = $"{Roles.Customer},{Roles.Admin}")]
        public async Task<ActionResult> Logout(CancellationToken cancellationToken = default)
        {
            await _userService.Logout(cancellationToken);
            return Ok(Language.LogOut);
        }
        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="user">The login model containing user credentials.</param>
        /// <param name="cancellation">Cancellation token for the operation.</param>
        /// <returns>
        /// 
        /// Status codes:
        /// 200 OK - If the login is successful.
        /// 404 Not Found - If the user does not exist.
        /// 403 Forbidden - If the user is banned.
        /// </returns>
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(LoginModel user, CancellationToken cancellation = default)
        {
            try
            {
                var username = await _userService.Login(user, cancellation);
                return Ok(JWTHelper.GenerateSecurityToken(_options, username.Claims));
            }
            catch (UserDoesnotExistException ex)
            {
                return NotFound(Language.UserDoesNotExists);
            }
            catch (UserIsBannedException)
            {
                return StatusCode(403, Language.UserDoesNotExists);
            }
            catch (PasswordIsNotCorrect ex)
            {
                return StatusCode(401, Language.PasswordIsNotCorrect);
            }

        }
        /// <summary>
        /// Bans an account by email.
        /// </summary>
        /// <param name="email">The email of the account to be banned.</param>
        /// <param name="duration">The duration of the ban in minutes.</param>
        /// <param name="cancellationToken">Cancellation token for asynchronous operation
        /// <returns>
        ///  status code:
        /// 200 OK - If the account is successfully banned.
        /// 404 Not Found - If the account does not exist.
        /// </returns>
        [Authorize(Roles = $"{Roles.Admin}")]
        [HttpPost("BanAccount/{email}")]
        public async Task<ActionResult> BanAccount(string email, int duration,CancellationToken cancellationToken)
        {
            try
            {
                await _userService.BanAccount(email, duration, cancellationToken);
                return Ok(Language.Ban);
            }
            catch (UserDoesnotExistException ex)
            {
                return NotFound(Language.UserDoesNotExists);
            }

        }
        /// <summary>
        /// Deletes an account by ID.
        /// </summary>
        /// <param name="id">The ID of the account to be deleted.</param>
        /// <param name="cancellationToken">Cancellation token for asynchronous operation
        /// <returns>
        /// Returns an with status code:
        /// 200 OK - If the account is successfully deleted.
        /// 404 Not Found - If the account does not exist.
        /// </returns>
        [Authorize(Roles = $"{Roles.Customer},{Roles.Admin}")]
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            try
            {
                await _userService.Delete(id);
                return Ok(Language.Delete);
            }
            catch (UserDoesnotExistException ex)
            {
                return NotFound(Language.UserDoesNotExists);
            }

        }
        /// <summary>
        /// Retrieves all user accounts.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token for the operation.</param>
        /// <returns>
        /// status code:
        /// 200 OK - If the operation is successful.
        /// </returns>
        [AllowAnonymous]
        [HttpGet("Accounts")]
        public async Task<ActionResult> GetAll(CancellationToken cancellationToken)
        {
            var users = await _userService.GetAll(cancellationToken);
            var result = users.Adapt<List<AccountResponseModel>>();
            return Ok(result);

        }
        /// <summary>
        /// Retrieves a user account by ID.
        /// </summary>
        /// <param name="id">The ID of the user account to retrieve.</param>
        /// <param name="cancellationToken">Cancellation token for asynchronous operation
        /// <returns>
        /// Returns an with status code:
        /// 200 OK - If the account is found.
        /// 404 Not Found - If the account does not exist.
        /// </returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(string id,CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userService.GetById(id, cancellationToken);
                var result = user.Adapt<AccountResponseModel>();
                return Ok(result);
            }
            catch (UserDoesnotExistException ex)
            {
                return NotFound(Language.UserDoesNotExists);
            }

        }
        /// <summary>
        /// Updates an account.
        /// </summary>
        /// <param name="accountModel">The updated account model.</param>
        /// <param name="cancellationToken">Cancellation token for asynchronous operation
        /// <returns>
        /// Returns an  with status code:
        /// 200 OK - If the account is successfully updated.
        /// 404 Not Found - If the account does not exist.
        /// </returns>
        [HttpPut("Update"), Authorize(Roles = $"{Roles.Customer},{Roles.Admin}")]
        public async Task<ActionResult> Update(AccountRequestmodel accountModel,CancellationToken cancellationToken)
        {
            try
            {
                await _userService.Update(accountModel, cancellationToken);
                return Ok(Language.Update);
            }
            catch (UserDoesnotExistException ex)
            {
                return NotFound(Language.Delete);
            }

        }
        /// <summary>
        /// Uploads an image for a user account.
        /// </summary>
        /// <param name="id">The ID of the user account.</param>
        /// <param name="cancellationToken">Cancellation token for asynchronous operation
        /// <returns>
        ///  status code:
        /// 200 OK - If the image is successfully uploaded.
        /// </returns>
        [HttpPost("Uploadimage/{id}"), Authorize(Roles = $"{Roles.Customer},{Roles.Admin}")]
        public async Task<ActionResult> UploadImage(string id,CancellationToken cancellationToken)
        {

            await _userService.UploadImage(id, cancellationToken);
            return Ok(Language.Create);

        }
        /// <summary>
        /// Retrieves a user account by email.
        /// </summary>
        /// <param name="email">The email of the user account to retrieve.</param>
        /// <param name="cancellationToken">Cancellation token for asynchronous operation
        /// <returns>
        /// status code:
        /// 200 OK - If the account is found.
        /// 404 Not Found - If the account does not exist.
        /// </returns>
        [AllowAnonymous]
        [HttpGet("User/{email}")]
        public async Task<ActionResult> GetByEmail(string email, CancellationToken cancellationToken)
        {
            try
            {
                var account = await _userService.GetByEmail(email, cancellationToken);
                var result = account.Adapt<AccountResponseModel>();
                return Ok(result);
            }
            catch (UserDoesnotExistException ex)
            {
                return NotFound(Language.UserDoesNotExists);
            }

        }
        /// <summary>
        /// Retrieves all comments for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="cancellationToken">Cancellation token for asynchronous operation
        /// <returns>
        ///  status code:
        /// 200 OK - If the operation is successful.
        /// 404 Not Found - If the user does not exist.
        [HttpGet("Comments"), Authorize(Roles = $"{Roles.Admin}")]
        public async Task<ActionResult> GetAllComments(string userId, CancellationToken cancellationToken)
        {
            try
            {
                var comments = await _userService.GetAllComments(userId, cancellationToken);

                var result = comments.Adapt<List<CommentResponseModel>>();
                return Ok(result);
            }
            catch (UserDoesnotExistException ex)
            {
                return NotFound(Language.UserDoesNotExists);
            }

        }
        /// <summary>
        /// Retrieves all articles for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="cancellationToken">Cancellation token for asynchronous operation
        /// <returns>
        ///  status code:
        /// 200 OK - If the operation is successful.
        /// 404 Not Found - If the user does not exist.
        /// </returns>
        [HttpGet("Articles"), Authorize(Roles = $"{Roles.Customer},{Roles.Admin}")]
        public async Task<ActionResult> GetAllArticles(string userId, CancellationToken cancellationToken)
        {
            try
            {
                var articles = await _userService.GetAllArticles(userId, cancellationToken);
                var result = articles.Adapt<List<ArticleResponse>>();
                return Ok(result);
            }
            catch (UserDoesnotExistException ex)
            {
                return NotFound(Language.UserDoesNotExists);
            }

        }

        /// <summary>
        /// Changes the password for the current user.
        /// </summary>
        /// <param name="password">The new password of the user.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>
        /// 
        /// Status codes:
        /// 200 OK - If the password change is successful.
        /// 400 Bad Request - If the new and confirmed passwords do not match.
        /// </returns>
        [HttpPost("ChangePassword"),Authorize]
        public async Task<ActionResult> ChangePassword(ChangePasswordModel password, CancellationToken cancellationToken = default)
        {
            try
            {
                await _userService.ChangePassword(password.oldPassword, password.newPassword, password.ConfirmPassword, cancellationToken);

                return Ok(Language.Update); 
            }
            catch (ConfirmedPasswordIsNotCorrect ex)
            {
                return BadRequest(Language.PasswordIsNotCorrect);
            }
        }

   
    }
}

