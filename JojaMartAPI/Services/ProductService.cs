using JojaMartAPI.Services.Interfaces;

namespace JojaMartAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly JojaMartDbContext _dbContext;

        public ProductService(JojaMartDbContext dbContext)
        {
            _dbContext = dbContext;
        }


    }
}
