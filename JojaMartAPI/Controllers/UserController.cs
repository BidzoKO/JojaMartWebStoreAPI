﻿using JojaMartAPI.DTOs.JwtDtos;
using JojaMartAPI.DTOs.UserDtos;
using JojaMartAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
        public async Task<ActionResult<AuthenticatedUserResponse>> UserLogin([FromBody] UserLoginDTO userCredentials)
        {
            try
            {
                var allUsers = _dbContext.Users.ToList();

                var user = allUsers.FirstOrDefault(e => e.Email == userCredentials.Email);



                if (user != null && BCrypt.Net.BCrypt.Verify(userCredentials.Password, user.PasswordHash))
                {
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
            var userStringId = HttpContext.User.FindFirstValue("Id");

            if (userStringId == null)
            {
                return BadRequest();
            }

            int.TryParse(userStringId, out int parsedId);

            await _tokenService.DeleteAllRefreshTokens(parsedId);

            return NoContent();
        }


        [HttpPost("RefreshJwt", Name = "RefreshJwt")]
        public async Task<IActionResult> RefreshAcessToken([FromBody] RefreshRequest refreshRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("invalid model state");
            }

            var tokenValidity = _tokenService.ValidateRefreshToken(refreshRequest.RefreshToken);

            if (!tokenValidity)
            {
                return BadRequest("invalid token");
            }

            UserRefreshToken? userRefreshTokenDbo = await _tokenService.GetTokenDto(refreshRequest.RefreshToken);

            if (userRefreshTokenDbo == null)
            {
                return NotFound("couldn't find refresh token");
            }

            User user = _dbContext.Users.FirstOrDefault(r => r.Id == userRefreshTokenDbo.Id)!;

            return Ok(await _tokenService.AuthenticateUser(user));

        }


        [HttpPost("CreateNewUser", Name = "CreateNewUser")]
        public ActionResult<User> CreateNewUser([FromBody] CreateUserDTO userData)
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
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(userData.Password),
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

