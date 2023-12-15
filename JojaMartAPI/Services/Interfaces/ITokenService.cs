using JojaMartAPI.Models;

namespace JojaMartAPI.Services.Interfaces
{
    public interface ITokenService
    {
        public string CreateJWT(User user);
    }
}
