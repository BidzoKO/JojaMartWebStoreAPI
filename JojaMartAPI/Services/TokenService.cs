using JojaMartAPI.Data;
using JojaMartAPI.Models;
using JojaMartAPI.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JojaMartAPI.Services
{
    public class TokenService : ITokenService
    {
        private readonly JojaMartDbContext _dbContext; IConfiguration _configuration;

        public TokenService(JojaMartDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public string CreateAcessJwt(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName ),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var expDate = DateTime.UtcNow.AddMinutes(15);

            return (GenerateJwt(expDate, claims));
        }

        public string CreateRefreshJwt()
        {
            var expDate = DateTime.UtcNow.AddMonths(3);

            return GenerateJwt(expDate);
        }

        private string GenerateJwt(DateTime expDate, IEnumerable<Claim>? claims = null)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:JwtConfiguration:SecretKey").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                "https://localhost:7177/",
                "https://localhost:7177/",
                claims,
                DateTime.UtcNow,
                expDate,
                creds
                );

            var newJwt = new JwtSecurityTokenHandler().WriteToken(token);

            return (newJwt);
        }

        public bool ValidateRefreshToken(string refreshToken)
        {
            TokenValidationParameters validationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:JwtConfiguration:SecretKey").Value!)),
                ValidAudience = "https://localhost:7177/",
                ValidIssuer = "https://localhost:7177/",
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ClockSkew = TimeSpan.Zero,
            };

            JwtSecurityTokenHandler TokenHandler = new JwtSecurityTokenHandler();

            try
            {
                TokenHandler.ValidateToken(refreshToken, validationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }
    }
}
