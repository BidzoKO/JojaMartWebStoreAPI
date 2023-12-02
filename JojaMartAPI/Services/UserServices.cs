using JojaMartAPI.Data;
using JojaMartAPI.Models;
using JojaMartAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JojaMartAPI.Services
{
    public class UserServices : IUserServices
    {
        private readonly JojaMartDbContext _dbContext;

        public UserServices(JojaMartDbContext dbContext)
        {
            _dbContext = dbContext;
        }





    }
}
