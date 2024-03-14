using Google.Cloud.Storage.V1;
using JojaMartAPI.DTOs.GenericDtos;
using JojaMartAPI.DTOs.JwtDtos;
using JojaMartAPI.DTOs.UserDtos;
using JojaMartAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JojaMartAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UserController : ControllerBase
	{
		private readonly JojaMartDbContext _dbContext; ITokenService _tokenService; IUserService _userService;

		public UserController(JojaMartDbContext dbContext, ITokenService tokenService, IUserService userService)
		{
			_dbContext = dbContext;
			_tokenService = tokenService;
			_userService = userService;
		}


		[HttpGet("GetAllUsers", Name = "GetAllUsers")]
		public ActionResult<List<User>> GetAllUsers()
		{
			try
			{
				var allUsers = _dbContext.Users.ToList();
				return Ok(allUsers);
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}

		}


		[HttpGet("GetUserById", Name = "GetUserById"), Authorize()]
		public ActionResult<User> GetUserById(int Id)
		{
			var _userDb = _dbContext.Users.ToList();

			var item = _userDb.FirstOrDefault(x => x.Id == Id);

			return Ok(item);

		}


		[HttpPost("UserLogin", Name = "UserLogin")]
		public async Task<ActionResult<AuthenticatedUserResponse>> UserLogin([FromBody] UserLoginDTO userCredentials)
		{
			try
			{
				var user = _dbContext.Users.FirstOrDefault(e => e.Email == userCredentials.Email);

				if (user != null && BCrypt.Net.BCrypt.Verify(userCredentials.Password, user.PasswordHash) && user.AccountStatus == "a")
				{
					user.LastLoginDate = DateTime.UtcNow;
					_dbContext.SaveChanges();

					return Ok(await _tokenService.AuthenticateUser(user));
				}
				else
				{
					return BadRequest("Wrong email or password or the user doesnt exist");
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}

		}


		[HttpDelete("Logout", Name = "Logout"), Authorize]
		public async Task<IActionResult> UserLogout()
		{
			var userStringId = int.Parse(this.User.Claims.First(i => i.Type == "Id").Value);


			if (userStringId == null)
			{
				return BadRequest();
			}

			await _tokenService.DeleteAllRefreshTokens(userStringId);

			return NoContent();
		}


		[HttpPost("RefreshJwt", Name = "RefreshJwt")]
		public async Task<IActionResult> RefreshAcessToken([FromBody] GenericStringDTO refreshToken)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest("invalid model state");
			}

			var tokenValidity = _tokenService.ValidateRefreshToken(refreshToken.StringValue);

			if (!tokenValidity)
			{
				return BadRequest("invalid token");
			}

			UserRefreshToken? userRefreshTokenDbo = await _tokenService.GetTokenDto(refreshToken.StringValue);

			if (userRefreshTokenDbo == null)
			{
				return NotFound("couldn't find refresh token");
			}

			User user = _dbContext.Users.FirstOrDefault(r => r.Id == userRefreshTokenDbo.UserId)!;

			return Ok(await _tokenService.AuthenticateUser(user));

		}


		[HttpPost("CreateNewUser", Name = "CreateNewUser")]
		public async Task<ActionResult<AuthenticatedUserResponse>> CreateNewUser([FromBody] CreateUserDTO userData)
		{
			try
			{
				if (ModelState.IsValid)
				{
					if (_dbContext.Users.Any(e => e.Username == userData.Username))
					{
						return Conflict(new { Message = "A user with the same name already exists." });
					}

					var newUser = new User
					{
						FirstName = userData.FirstName,
						LastName = userData.LastName,
						Username = userData.Username,
						Email = userData.Email,
						PasswordHash = BCrypt.Net.BCrypt.HashPassword(userData.Password),
						Dob = userData.Dob,
						Gender = userData.Gender,
						Address = userData.Address,// nullable
						PhoneNumber = userData.PhoneNumber,// nullable
						CallingCode = userData.CallingCode,// nullable
						RegistrationDate = DateTime.UtcNow,
						LastLoginDate = DateTime.UtcNow,
						AccountStatus = "a",
						ProfilePictureUrl = null,
					};

					_dbContext.Users.Add(newUser);

					_dbContext.SaveChanges();

					return Ok(await _tokenService.AuthenticateUser(newUser));
				}
				return BadRequest(ModelState);
			}
			catch (DbUpdateException ex)
			{
				return BadRequest("Database constraint violation: " + ex.Message);
			}
		}


		[HttpPost("GetUserInfo", Name = "GetUserInfo"), Authorize]
		public async Task<ActionResult<UserIdentityDTO>> GetUserInfo()
		{
			try
			{
				var userId = int.Parse(this.User.Claims.First(i => i.Type == "Id").Value);

				var userDto = await _userService.GetUserIdentityByIdAsync(userId);

				return Ok(userDto);
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
		}


		[HttpPut("UpdateUser"), Authorize]
		public async Task<ActionResult> UpdateUser([FromBody] UpdateUserDTO updatedUser)
		{
			try
			{
				var userId = int.Parse(this.User.Claims.First(i => i.Type == "Id").Value);
				var userObject = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

				if (userObject == null)
					return BadRequest("user not found");

				var userNameSubstring = updatedUser.UserFullName.Split(" ");

				if (userNameSubstring is not { Length: 2 })
					userNameSubstring = new string[] { userObject.FirstName, userObject.LastName };

				var updatedUsername = string.IsNullOrEmpty(updatedUser.Username) ? userObject.Username : updatedUser.Username;
				var updatedAddress = string.IsNullOrEmpty(updatedUser.Address) ? userObject.Address : updatedUser.Address;
				var updatedPhone = string.IsNullOrEmpty(updatedUser.UserPhone) ? userObject.PhoneNumber : updatedUser.UserPhone;
				var updatedPassword = string.IsNullOrEmpty(updatedUser.UserPassword) ? userObject.PasswordHash : BCrypt.Net.BCrypt.HashPassword(updatedUser.UserPassword);

				userObject.FirstName = userNameSubstring[0];
				userObject.LastName = userNameSubstring[1];
				userObject.Username = updatedUsername;
				userObject.PasswordHash = updatedPassword;
				userObject.Address = updatedAddress;
				userObject.PhoneNumber = updatedPhone;

				_dbContext.SaveChanges();

				return Ok();
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Error uploading image: {ex.Message}");
			}
		}

		[HttpPatch("UpdateUserProfile"), Authorize]
		public async Task<ActionResult> UpdateUserProfile(IFormCollection userProfileForm)
		{
			var newProfilePicture = userProfileForm.Files[0];
			var userId = int.Parse(this.User.Claims.First(i => i.Type == "Id").Value);

			var userObject = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

			if (userObject == null)
				return BadRequest("user not found");

			var updatedImageUrl = userObject.ProfilePictureUrl;

			if (newProfilePicture is { Length: > 0 })
			{
				using var memoryStream = new MemoryStream();
				await newProfilePicture.CopyToAsync(memoryStream);
				var storage = StorageClient.Create();
				var objectName = $"{Guid.NewGuid()}-{newProfilePicture.FileName}";
				var imageObject = await storage.UploadObjectAsync("jojamart-user-profiles", objectName, null, memoryStream);
				updatedImageUrl = $"https://storage.googleapis.com/jojamart-user-profiles/{objectName}";

				userObject.ProfilePictureUrl = updatedImageUrl;
				await _dbContext.SaveChangesAsync();
				return Ok();
			}


			return NoContent();
		}
	}
}

