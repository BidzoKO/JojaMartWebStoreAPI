using JojaMartAPI.Models;

namespace JojaMartAPI.Services.Interfaces
{
    public interface ITokenService
    {
        public string CreateAcessJwt(User user);
        public string CreateRefreshJwt();
        public bool ValidateRefreshToken(string refreshToken);

    }
}
