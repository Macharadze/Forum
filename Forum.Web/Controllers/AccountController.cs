using Forum.Application.AccountModel;
using Forum.Application.Exceptions;
using Forum.Application.IUser;
using Forum.Domain.Roles;
using Forum.Web.Infrastructure.Validations;
using Forum.Web.Models;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        public async Task<ActionResult> LogOut(CancellationToken token)
        {
            await _userService.Logout(token);
            return RedirectToAction("Articles", "Article");

        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginModel login, CancellationToken token)
        {
            if (!ModelState.IsValid)
                return View();
            try
            {
                var user = await _userService.Login(login, token);
                if (user != null)
                {

                    return RedirectToAction("Articles", "Article");
                }
                ModelState.AddModelError("", "Username or password is incorrect");
            }
            catch (UserDoesnotExistException)
            {
                ModelState.AddModelError("", "Username or password is incorrect");

                // TempData["ErrorMessage"] = "User does not exists";
            }
            catch (UserIsBannedException)
            {
                TempData["ErrorMessage"] = "User is bannned";
            }
            catch (PasswordIsNotCorrect)
            {
                ModelState.AddModelError("", "Username or password is incorrect");
                // TempData["ErrorMessage"] = "Password is incorrect";
            }

            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Register([FromForm] RegisterModel register, CancellationToken token)
        {
            if (!ModelState.IsValid)
                return View();
            try
            {
                var validation = new UserVlidation();
                var result = await validation.ValidateAsync(register);
                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.ErrorMessage);

                    }
                    return View();
                }
                await _userService.Create(register, token);
                await _userService.Login(new LoginModel() { UserName = register.Username, Password = register.Password }, token);

                return RedirectToAction("Articles", "Article");
            }
            catch (UserAlreadyExistException ex)
            {
                TempData["ErrorMessage"] = "User aleady exists";
                return View();
            }

        }
        public IActionResult BanAccount()
        {
            return View();
        }

        [HttpPost, Authorize(Roles = $"{Roles.Admin}")]
        public async Task<ActionResult> BanAccount([FromForm] BanAccountViewModel ban, CancellationToken token)
        {
            if (!ModelState.IsValid)
                return View();
            try
            {
                await _userService.BanAccount(ban.Email, ban.Duration, token);

                return View();
            }
            catch (UserDoesnotExistException ex)
            {
                TempData["ErrorMessage"] = "User does not exists";
                return View();
            }


        }


        public IActionResult Update()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Update([FromForm] AccountRequestmodel account, CancellationToken token)
        {
            if (!ModelState.IsValid)
                return View();
            try
            {
                var current = await _userService.GetCurrentUser(token);
                await _userService.Update(account, token);
                return View();
            }
            catch (UserDoesnotExistException ex)
            {
                TempData["ErrorMessage"] = "User does not exists";
                return View();
            }

        }
        public IActionResult ChangePassword()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> ChangePassword([FromForm] string currentPassword, string newPassword, string confirmPassword, string email, CancellationToken token)
        {
            try
            {
                await _userService.ChangePassword(currentPassword, newPassword, confirmPassword, token);

                return RedirectToAction("Articles", "article");
            }
            catch (ConfirmedPasswordIsNotCorrect ex)
            {
                TempData["ErrorMessage"] = "Confirmed Password is not correct";
                return View();
            }
        }

        public IActionResult UploadImage()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> UploadImage([FromForm] PhotoRequest photo, CancellationToken token)
        {
            if (!ModelState.IsValid)
                return View();
            var currentUser = await _userService.GetCurrentUser(token);
            await _userService.UploadImage(currentUser.Email.ToString(), token);

            return View();

        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetProfile(string Email, bool isAuth, CancellationToken token)
        {
            try
            {
                var viewModel = new ProfileViewModel();

                if (string.IsNullOrEmpty(Email) && isAuth)
                {
                     
                    var currenUser = await _userService.GetCurrentUser(token);
                    viewModel.IsOwner = true;
                    var result = currenUser.Adapt<AccountResponseModel>();
                    viewModel.AccountResponseModel = result;
                }
                else
                {
                    var User = await _userService.GetByEmail(Email, token);
                    var result = User.Adapt<AccountResponseModel>();
                    viewModel.AccountResponseModel = result;

                    if (isAuth)
                    {
                        var currenUser = await _userService.GetCurrentUser(token);
                        if (User.Equals(currenUser))
                        {
                            viewModel.IsOwner = true;
                        }
                    }
                }
                return View(viewModel);
            }
            catch (UserDoesnotExistException ex)
            {
                TempData["ErrorMessage"] = "User does not exists";
                return RedirectToAction("Articles", "article");
            }

        }

        [HttpGet]
        public async Task<ActionResult> GetPhoto(string path)
        {
            if (path == null)
            {
                return View();
            }
            else
            {
                var imageFileStream = System.IO.File.OpenRead(path);
                return File(imageFileStream, "image/png");
            }
        }
    }
}
