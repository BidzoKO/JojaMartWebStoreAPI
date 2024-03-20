using JojaMartAPI.DTOs.UserDtos;

namespace JojaMartAPI.Services.Interfaces
{
    public interface IUserService
    {
        public Task<UserIdentityDTO> GetUserIdentityByIdAsync(int userId);
    }
}
