using Microsoft.AspNetCore.Mvc;

namespace JojaMartAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly JojaMartDbContext _dbContext;

        public ProductController(JojaMartDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("getLimitedTimeItems")]
        public async Task<IActionResult> GetLimitedTimeItems()
        {
            try
            {
                var itemIds = new List<int>()
                {
                2, 4, 8, 15, 20, 24, 31, 32, 36
                };

                var limitedProducts = _dbContext.Products.Where(r => itemIds.Contains(r.Id)).ToList();

                return Ok(limitedProducts);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        [HttpGet("getPopularItems")]
        public async Task<IActionResult> GetPopualrItems()
        {
            try
            {
                var itemIds = new List<int>()
                {
                6, 12, 13, 19, 25, 32, 35, 37, 38
                };

                var popularProducts = _dbContext.Products.Where(r => itemIds.Contains(r.Id)).ToList();

                return Ok(popularProducts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("getProductByName")]
        public async Task<IActionResult> GetProductByName([FromBody] string productName)
        {
            try
            {
                var product = _dbContext.Products.FirstOrDefault(r => r.ProductName == productName);

                if (product == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(product);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }



    }
}
