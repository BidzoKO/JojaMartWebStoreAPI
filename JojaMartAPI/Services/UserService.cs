using JojaMartAPI.Data;
using JojaMartAPI.Services.Interfaces;

namespace JojaMartAPI.Services
{
    public class UserService : IUserService
    {
        private readonly JojaMartDbContext _dbContext;

        public UserService(JojaMartDbContext dbContext)
        {
            _dbContext = dbContext;
        }

    }
}
