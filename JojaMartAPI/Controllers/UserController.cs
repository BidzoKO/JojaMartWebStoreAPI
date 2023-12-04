using JojaMartAPI.Data;
using JojaMartAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JojaMartAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly JojaMartDbContext _dbContext;

        public UserController(JojaMartDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet(Name = "GetAllUsers")]
        public ActionResult<List<User>> GetAllUsers()
        {
            var allUsers = _dbContext.Users.ToList();
            return Ok(allUsers);
        }

        [HttpGet("Id", Name = "GetUserById")]
        public ActionResult<User> GetUserById(int Id)
        {
            var _userDb = _dbContext.Users.ToList();

            var item = _userDb.FirstOrDefault(x => x.Id == Id);

            return Ok(item);

        }
  
        [HttpGet("Data",Name = "UserLogin")]
        public ActionResult<User> UserLogin(string userEmail, string userPassword)
        {
            try
            {
                var allUsers = _dbContext.Users.ToList();
                foreach (var user in allUsers)
                {
                    if (user.Email == userEmail && user.Password == userPassword)
                    {
                        return Ok(user);
                    }
                    else
                    {
                        return BadRequest("no user found, or wrong credentials");
                    }
                }
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
      
        }

        [HttpPost(Name = "CreateNewUser")]
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
                        Password = userData.Password,
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

