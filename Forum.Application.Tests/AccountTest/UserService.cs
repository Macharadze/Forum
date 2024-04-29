using FluentAssertions;
using Forum.Application.AccountModel;
using Forum.Application.Exceptions;
using Forum.Application.IUser;
using Forum.Application.Tests.Fixture;
using Forum.Application.UOW;
using Forum.Domain.Bans;
using Forum.Domain.Comments;
using Moq;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Assert = Xunit.Assert;

namespace Forum.Application.Tests.Account
{
    [ExcludeFromCodeCoverage]
    public class UserService
    {
        private readonly Mock<IUserUOW> _ouw = new Mock<IUserUOW>();
        private readonly IUserService _userService;
        private readonly IDfixture _fixture;
        public UserService()
        {
            _fixture = new IDfixture();
            _userService = new UserSerrvice(_ouw.Object);

        }
        [Fact]
        public async Task CreateUser_ShouldCreate_WhenUserIsCorrect()
        {
            // Arrange
            var registerModel = new RegisterModel
            {
                Username = "testuser",
                Email = "test@example.com",
                Gender = true,
                Phone = "1234567890",
                Password = "Password123"
            };

            _ouw.Setup(u => u._userRepository.Create(registerModel, default)).Returns(Task.CompletedTask);

            // Act
            await _userService.Create(registerModel);

            // Assert
            _ouw.Verify(u => u._userRepository.Create(registerModel, default), Times.Once);
        }
        [Fact]
        public async Task CreateUser_ShouldThrowUserDoesnotExistException_WhenUserExist()
        {
            // Arrange
            var registerModel = new RegisterModel { Email = "test@example.com" };

            _ouw.Setup(x => x._userRepository.ExistByEmail(registerModel.Email, default)).ReturnsAsync(true);

            _ouw.Setup(u => u._userRepository.Create(registerModel, default))
                .ThrowsAsync(new UserAlreadyExistException("User already exists"));

            // Act & Assert
            await Assert.ThrowsAsync<UserAlreadyExistException>(() => _userService.Create(registerModel));
        }
        [Fact]
        public async Task GetByID_ShouldReturnAccount_WhneUserIsExist()
        {
            _ouw.Setup(x => x._userRepository.ExistById(It.IsAny<string>(), default)).ReturnsAsync(true);
            _ouw.Setup(x => x._userRepository.GetById(It.IsAny<string>(), default)).ReturnsAsync(It.IsAny<Domain.Accounts.Account>());

            var result = await _userService.GetById(It.IsAny<string>(), default);

            result.Should().Be(It.IsAny<Domain.Accounts.Account>());
        }
        [Fact]
        public async Task GetByID_ShouldThrowException_WhneUserNotExist()
        {
            _ouw.Setup(x => x._userRepository.GetById(It.IsAny<string>(), default)).ThrowsAsync(new UserDoesnotExistException("User already exists"));

            await Assert.ThrowsAsync<UserDoesnotExistException>(() => _userService.GetById(It.IsAny<string>(), default));

        }
        [Fact]
        public async Task GetByEmail_ShouldReturnAccount_WhneUserIsExist()
        {
            _ouw.Setup(x => x._userRepository.ExistByEmail(It.IsAny<string>(), default)).ReturnsAsync(true);
            _ouw.Setup(x => x._userRepository.GetByEmail(It.IsAny<string>(), default)).ReturnsAsync(It.IsAny<Domain.Accounts.Account>());

            var result = await _userService.GetByEmail(It.IsAny<string>(), default);

            result.Should().Be(It.IsAny<Domain.Accounts.Account>());
        }
        [Fact]
        public async Task GetByEmail_ShouldThrowException_WhneUserNotExist()
        {
            _ouw.Setup(x => x._userRepository.GetByEmail(It.IsAny<string>(), default)).ThrowsAsync(new UserDoesnotExistException("User already exists"));

            await Assert.ThrowsAsync<UserDoesnotExistException>(() => _userService.GetById(It.IsAny<string>(), default));

        }
        [Fact]
        public async Task DeleteUser_ShouldDeleteUser_WhenUserExists()
        {

            _ouw.Setup(x => x._userRepository.ExistById(It.IsAny<string>(), default)).ReturnsAsync(true);
            _ouw.Setup(u => u._userRepository.Delete(It.IsAny<string>(), default)).Returns(Task.CompletedTask);

            // Act
            await _userService.Delete(It.IsAny<string>(), default);

            _ouw.Verify(u => u._userRepository.Delete(It.IsAny<string>(), default), Times.Once);


        }
        [Fact]
        public async Task DeleteUser_ShouldThrowUserDoesnotExistException_WhenUserDoesNotExist()
        {

            _ouw.Setup(x => x._userRepository.GetById(It.IsAny<string>(), default)).ThrowsAsync(new UserDoesnotExistException("User already exists"));

            await Assert.ThrowsAsync<UserDoesnotExistException>(() => _userService.Delete(It.IsAny<string>(), default));
        }
        [Fact]
        public async Task DeleteImage_ShouldDeleteImage_WhenUserExists()
        {
            // Arrange

            _ouw.Setup(x => x._userRepository.ExistById(It.IsAny<string>(), default)).ReturnsAsync(true);
            _ouw.Setup(x => x._userRepository.GetById(It.IsAny<string>(), default)).ReturnsAsync(It.IsAny<Forum.Domain.Accounts.Account>());

            // Act
            await _userService.DeleteIMage(It.IsAny<string>(), default);
            _ouw.Verify(x => x._userRepository.DeleteIMage(It.IsAny<Forum.Domain.Accounts.Account>(), default), Times.Once);
        }
        [Fact]
        public async Task DeleteImage_ShouldThrowUserDoesnotExistException_WhenUserDoesNotExist()
        {
            // Arrange

            _ouw.Setup(x => x._userRepository.ExistById(It.IsAny<string>(), default)).ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<UserDoesnotExistException>(() => _userService.DeleteIMage(It.IsAny<string>(), default));
        }
        [Fact]
        public async Task GetAll_ShouldReturnAllAccounts_WhenExecutionIsSuccessful()
        {
            // Arrange
            /* var cancellationToken = CancellationToken.None;
             var expectedAccounts = new List<Account> { *//* create sample accounts for testing *//* };*/
            var comments = new List<Forum.Domain.Accounts.Account>();
            _ouw.Setup(x => x._userRepository.GetAll(default)).ReturnsAsync(comments);

            // Act
            var result = await _userService.GetAll(default);

            // Assert
            // result.Should().NotBeEmpty();
            result.Should().BeEquivalentTo(comments);
        }
        [Fact]
        public async Task Update_ShouldUpdateUser_WhenUpdateIsSuccessful()
        {
            // Arrange

            _ouw.Setup(x => x._userRepository.Update(It.IsAny<AccountRequestmodel>(), default)).Returns(Task.CompletedTask);

            // Act
            await _userService.Update(It.IsAny<AccountRequestmodel>(), default);


            // Assert
            _ouw.Verify(x => x._userRepository.Update(It.IsAny<AccountRequestmodel>(), default), Times.Once);

        }
        [Fact]
        public async Task GetAllComments_ShouldReturnAllComments_WhenUserExists()
        {
            // Arrange
            var expectedComments = new List<Comment>() { new Comment() { Content = "aa" } };
            _ouw.Setup(x => x._userRepository.ExistById(It.IsAny<string>(), default)).ReturnsAsync(true);
            _ouw.Setup(x => x._userRepository.GetAllComments(It.IsAny<string>(), default)).ReturnsAsync(expectedComments);

            // Act
            var result = await _userService.GetAllComments(It.IsAny<string>(), default);

            // Assert
            result.Should().BeEquivalentTo(expectedComments);
            result.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetAllComments_ShouldThrowUserDoesnotExistException_WhenUserDoesNotExist()
        {
            // Arrange

            _ouw.Setup(x => x._userRepository.ExistById(It.IsAny<string>(), default)).ReturnsAsync(false);

            // Act & Assert

            await Assert.ThrowsAsync<UserDoesnotExistException>(() => _userService.GetAllComments(It.IsAny<string>(), default));
        }


        [Fact]
        public async Task BanAccount_ShouldBanUser_WhenUserExists()
        {
            var email = "test@example.com";
            var duration = 5;
            var targetId = _fixture._guid;
            var banStartDate = DateTime.Now;
            var banEndDate = banStartDate.AddMinutes(duration);

            _ouw.Setup(x => x._userRepository.BanAccount(email, default)).ReturnsAsync(targetId.ToString());
            _ouw.Setup(x => x._banAccountRepository.Add(It.Is<AccountBan>(ban =>
                ban.StartDate == banStartDate &&
                ban.EndDate == banEndDate &&
                ban.AccountID == targetId.ToString()), default)).Returns(Task.CompletedTask);
            // Act
            await _userService.BanAccount(email, duration, default);

            // Assert
            _ouw.Verify(x => x._userRepository.BanAccount(email, default), Times.Once);

        }

        [Fact]
        public async Task UnBanAccount_ShouldRemoveBans_WhenBansAreExpired()
        {
            var id = _fixture._guid.ToString();
            // Arrange
            var banAccounts = new List<AccountBan>
            {
                new AccountBan
                {
                    AccountID = id
                }
            };

            _ouw.Setup(x => x._banAccountRepository.GetAll(default)).ReturnsAsync(banAccounts);
            _ouw.Setup(x => x._banAccountRepository.CheckIfExpired(It.IsAny<DateTime>(), default)).ReturnsAsync(true);
            _ouw.Setup(x => x._userRepository.GetById(id, default)).ReturnsAsync(It.IsAny<Forum.Domain.Accounts.Account>());
            // Act
            await _userService.UnBanAccount(default);

            // Assert
            foreach (var ban in banAccounts)
            {
                _ouw.Verify(x => x._userRepository.UnBanAccount(ban.AccountID, default), Times.Once);
            }
        }
        [Fact]
        public async Task Login_ShouldReturnCustomer_WhenLoginIsSuccessful()
        {
            // Arrange
            var loginModel = new LoginModel { UserName = "customer", Password = "test" };
            var claims = new List<Claim> { new Claim(ClaimTypes.Role, "Customer") };
            var expectedResponse = new JWTresponseModel { Claims = claims };

            _ouw.Setup(x => x._userRepository.Login(loginModel, default)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _userService.Login(loginModel, default);

            // Assert
            result.Should().NotBeNull();
            result.Claims.Should().BeEquivalentTo(claims);

        }
        [Fact]
        public async Task Login_ShouldReturnAdmin_WhenLoginIsSuccessful()
        {
            // Arrange
            var loginModel = new LoginModel { UserName = "admin" };
            var claims = new List<Claim> { new Claim(ClaimTypes.Role, "Admin") };
            var expectedResponse = new JWTresponseModel { Claims = claims };

            _ouw.Setup(x => x._userRepository.Login(loginModel, default)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _userService.Login(loginModel, default);

            // Assert
            result.Should().NotBeNull();
            result.Claims.Should().BeEquivalentTo(claims);

        }
        [Fact]
        public async Task Login_ShouldThrowException_WhenLoginFails()
        {
            // Arrange
            var loginModel = new LoginModel { UserName = "User" };

            _ouw.Setup(x => x._userRepository.Login(loginModel, default)).ThrowsAsync(new UserDoesnotExistException("Failed to login"));

            // Act & Assert
            await Assert.ThrowsAsync<UserDoesnotExistException>(() => _userService.Login(loginModel, default));
        }
        [Fact]
        public async Task Login_ShouldThrowException_WhenPasswordIsIncorrect()
        {
            // Arrange
            var loginModel = new LoginModel { UserName = "User" };

            _ouw.Setup(x => x._userRepository.Login(loginModel, default)).ThrowsAsync(new PasswordIsNotCorrect(""));

            // Act & Assert
            await Assert.ThrowsAsync<PasswordIsNotCorrect>(() => _userService.Login(loginModel, default));
        }

        [Fact]
        public async Task UploadImage_ShouldUploadImage_WhenUserExists()
        {
            // Arrange
            var email = "test@example.com";
            var account = new Forum.Domain.Accounts.Account() { Email = email };

            _ouw.Setup(x => x._userRepository.ExistByEmail(email, default)).ReturnsAsync(true);
            _ouw.Setup(x => x._userRepository.GetByEmail(email, default)).ReturnsAsync(account);

            // Act
            await _userService.UploadImage(email, default);

            // Assert
            _ouw.Verify(x => x._userRepository.AddIMage(account, default), Times.Once);
        }
        [Fact]
        public async Task UploadImage_ShouldThrowUserDoesnotExistException_WhenUserDoesNotExist()
        {
            var email = "test@example.com";
            var account = new Forum.Domain.Accounts.Account() { Email = email };

            _ouw.Setup(x => x._userRepository.ExistByEmail(email, default)).ReturnsAsync(false);

            // Assert
            await Assert.ThrowsAsync<UserDoesnotExistException>(() => _userService.UploadImage(email, default));

        }
        [Fact]
        public async Task ChangePassword_ShouldChangePassword_WhenPasswordsMatch()
        {
            // Arrange
            var oldPassword = "OldPassword123";
            var newPassword = "NewPassword456";
            var confirmPassword = "NewPassword456";

            var user = new Domain.Accounts.Account(); 

            _ouw.Setup(u => u._userRepository.ChangePassword(user, oldPassword, newPassword, default))
                .Returns(Task.CompletedTask);

            _ouw.Setup(u => u._userRepository.GetCurrentUser(default))
                .ReturnsAsync(user);

            // Act
            await _userService.ChangePassword(oldPassword, newPassword, confirmPassword);

            // Assert
            _ouw.Verify(u => u._userRepository.ChangePassword(user, oldPassword, newPassword, default), Times.Once);
        }

        [Fact]
        public async Task ChangePassword_ShouldThrowConfirmedPasswordIsNotCorrectException_WhenPasswordsDoNotMatch()
        {
            // Arrange
            var oldPassword = "OldPassword123";
            var newPassword = "NewPassword456";
            var confirmPassword = "WrongPassword789";

            // Act & Assert
            await Assert.ThrowsAsync<ConfirmedPasswordIsNotCorrect>(() =>
                _userService.ChangePassword(oldPassword, newPassword, confirmPassword));
        }
        [Fact]
        public async Task Logout_ShouldCallLogoutMethod()
        {
            // Arrange
            _ouw.Setup(u => u._userRepository.Logout(default))
                .Returns(Task.CompletedTask);

            // Act
            await _userService.Logout();

            // Assert
            _ouw.Verify(u => u._userRepository.Logout(default), Times.Once);
        }

        [Fact]
        public async Task GetCurrentUser_ShouldReturnCurrentUser()
        {
            // Arrange
            var expectedCurrentUser = new Domain.Accounts.Account();

            _ouw.Setup(u => u._userRepository.GetCurrentUser(default))
                .ReturnsAsync(expectedCurrentUser);

            // Act
            var actualCurrentUser = await _userService.GetCurrentUser();

            // Assert
            Assert.Equal(expectedCurrentUser, actualCurrentUser);
        }
    }

}
