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

        public UserController (JojaMartDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet(Name ="GetAllUsers")]
        public ActionResult<List<User>> GetAllUsers()
        {               
            var allUsers = _dbContext.Users.ToList() ;            
            return Ok(allUsers);
        }

    }
}
