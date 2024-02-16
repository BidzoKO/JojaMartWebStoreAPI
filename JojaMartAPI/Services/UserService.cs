using JojaMartAPI.DTOs.UserDtos;
using JojaMartAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JojaMartAPI.Services
{
	public class UserService : IUserService
	{
		private readonly JojaMartDbContext _dbContext;

		public UserService(JojaMartDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<UserIdentityDTO> GetUserIdentityByIdAsync(int userId)
		{
			var userDbo = await _dbContext.Users.FirstOrDefaultAsync(c => c.Id == userId);

			if (userDbo == null)
			{
				throw new InvalidOperationException("User not found in the database");
			}

			var userDto = new UserIdentityDTO
			{
				FirstName = userDbo.FirstName,
				LastName = userDbo.LastName,
				Username = userDbo.Username,
				Email = userDbo.Email,
				Dob = userDbo.Dob,
				Gender = userDbo.Gender,
				Address = userDbo.Address,
				PhoneNumber = userDbo.PhoneNumber,
				CallingCode = userDbo.CallingCode,
				AccountStatus = userDbo.AccountStatus,
				ProfilePictureUrl = userDbo.ProfilePictureUrl,
			};

			return userDto;
		}
	}
}
