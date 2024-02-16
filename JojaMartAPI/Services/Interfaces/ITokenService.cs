
using JojaMartAPI.DTOs.JwtDtos;

namespace JojaMartAPI.Services.Interfaces
{
    public interface ITokenService
    {
        public string GenerateAcessJwt(User user);
        public string GenerateRefreshJwt();
        public bool ValidateRefreshToken(string refreshToken);
        public Task StoreRefreshToken(UserRefreshToken token);
        public Task<UserRefreshToken?> GetTokenDto(string token);
        public Task<UserRefreshToken> GetRefreshTokenDboByAccessToken(string accessToken);
        public Task<AuthenticatedUserResponse> AuthenticateUser(User user);
        public Task DeleteAllRefreshTokens(int userId);
        public bool IsTokenExpired(string Token);
        public int GetUserIdFromToken(string userToken);

    }
}
