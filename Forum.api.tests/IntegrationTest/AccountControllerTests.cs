using Bogus;
using FluentAssertions;
using Forum.Application.AccountModel;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace Forum.api.tests.IntegrationTest
{
    public class AccountControllerTests : IClassFixture<AcccountWebApplicationFactory>
    {

        private readonly AcccountWebApplicationFactory _factory;
        private readonly HttpClient _httpClient;
        private readonly Faker<RegisterModel> _faker =
             new Faker<RegisterModel>()
             .RuleFor(i => i.Username, faker => faker.Person.FirstName)
             .RuleFor(i => i.Email, faker => faker.Person.Email)
            .RuleFor(i => i.Gender, true)
            .RuleFor(i => i.Password, "Tbc123@")
            .RuleFor(i => i.Phone, "574045774");



        public AccountControllerTests(AcccountWebApplicationFactory factory)
        {
            _factory = factory;
            _httpClient = factory.CreateClient();
        }


        [Fact]
        public async Task CreateUser_ReturnOk()
        {
            var acc = _faker.Generate();

            var response = await _httpClient.PostAsJsonAsync("api/v1/Account/Register", acc);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

        }
        [Fact]
        public async Task GetAccount_WithoutAuth_Returns402()
        {

            var response = await _httpClient.GetAsync("api/v1/Account/Accounts");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        }

        [Fact]
        public async Task GetAccount_WithAuth_ReturnsAccounts()
        {
            // Arrange
            var username = "Darin";
            var password = "Tbc123@";
            var acc = new LoginModel()
            {
                UserName = username,
                Password = password
            };
            var loginContent = new StringContent(JsonConvert.SerializeObject(acc), Encoding.UTF8, "application/json");
            var loginResponse = await _httpClient.PostAsync("api/v1/Account/Login", loginContent);

            //  loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var loginResponseString = await loginResponse.Content.ReadAsStringAsync();


            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", loginResponseString.ToString());
            // Act
            var response = await _httpClient.GetAsync("api/v1/Account/Accounts");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);


        }
        [Fact]
        public async Task WhenCustomerExist_ReturnTheAccount()
        {
            string id = "39F60CA9-28DB-4440-2C91-08DC7E7DB531";

            var response = await _httpClient.GetAsync("api/v1/Account/" + id);

            var responseString = await response.Content.ReadAsStringAsync();
            var account = JsonConvert.DeserializeObject<AccountResponseModel>(responseString);

            response.Content.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            account?.UserName.Should().Be("Admin");
        }
        [Fact]
        public async Task WhenCustomerNotExist_ReturnErrors()
        {
            string id = "39F60CA9-28DB-4d40-2C91-08DC7E7DB431";

            var response = await _httpClient.GetAsync("api/v1/Account/" + id);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);


        }

    }
}