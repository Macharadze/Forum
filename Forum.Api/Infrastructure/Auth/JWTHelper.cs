using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Forum.Api.Infrastructure.Auth
{
    public  static class JWTHelper
    {
        public static string GenerateSecurityToken(IOptions<JWTConfiguration> options,List<Claim> claims)
        {
            var tokenhandler = new JwtSecurityTokenHandler();


            var key = Encoding.ASCII.GetBytes(options.Value.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims.ToArray()),
                Expires = DateTime.Now.AddDays(options.Value.ExpirationInMinutes),
                Audience = "localhost",
                Issuer = "localhost",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenhandler.CreateToken(tokenDescriptor);

            return tokenhandler.WriteToken(token);
        }
    }
}
