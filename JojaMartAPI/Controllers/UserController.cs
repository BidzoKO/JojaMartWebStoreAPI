using JojaMartAPI.Data;
using JojaMartAPI.DTOs.JwtDtos;
using JojaMartAPI.DTOs.UserDtos;
using JojaMartAPI.Models;
using JojaMartAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JojaMartAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase, IUserService
    {
        private readonly JojaMartDbContext _dbContext; ITokenService _tokenService;

        public UserController(JojaMartDbContext dbContext, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _dbContext = dbContext;
        }

        [HttpGet("GetAllUsers", Name = "GetAllUsers")]
        public ActionResult<List<User>> GetAllUsers()
        {
            var allUsers = _dbContext.Users.ToList();
            return Ok(allUsers);
        }

        [HttpGet("GetUserById", Name = "GetUserById"), Authorize()]
        public ActionResult<User> GetUserById(int Id)
        {
            var _userDb = _dbContext.Users.ToList();

            var item = _userDb.FirstOrDefault(x => x.Id == Id);

            return Ok(item);

        }

        [HttpPost("UserLogin", Name = "UserLogin")]
        public ActionResult<AuthenticatedUserResponse> UserLogin([FromBody] UserLoginDTO userCredentials)
        {
            try
            {
                var allUsers = _dbContext.Users.ToList();

                var user = allUsers.FirstOrDefault(e => e.Email == userCredentials.Email);

                if (user != null && BCrypt.Net.BCrypt.Verify(userCredentials.Password, user.Password))
                {
                    var AcessToken = _tokenService.CreateJWT(user);

                    return Ok(new AuthenticatedUserResponse()
                    {
                        AccessJwt = AcessToken,
                        RefreshJwt = null,//need to change
                    });

                }
                else
                {
                    return BadRequest("Wrong email or password");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("CreateNewUser", Name = "CreateNewUser")]
        public ActionResult<User> CreateNewUser(CreateUserDTO userData)
        {
            try
            {
                var _userDb = _dbContext.Users;
                if (ModelState.IsValid)
                {

                    var newUser = new User
                    {
                        FirstName = userData.FirstName,
                        LastName = userData.LastName,
                        Username = userData.Username,
                        Email = userData.Email,
                        Password = BCrypt.Net.BCrypt.HashPassword(userData.Password),
                        Dob = userData.Dob,
                        Gender = userData.Gender,
                        Address = userData.Address,
                        PhoneNumber = userData.PhoneNumber,
                        CallingCode = userData.CallingCode,
                        RegistrationDate = DateTime.Now,
                        LastLoginDate = DateTime.Now,
                        AccountStatus = "a",
                        ProfilePictureUrl = null,
                    };

                    _userDb.Add(newUser);

                    _dbContext.SaveChanges();

                    return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
                }
                return BadRequest(ModelState);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest("Database constraint violation: " + ex.Message);
            }
        }

    }
}

