using JojaMartAPI.DTOs.OrderDtos;
using JojaMartAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JojaMartAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {

        private readonly JojaMartDbContext _dbContext; IOrderService _orderService;

        public OrderController(JojaMartDbContext dbContext, IOrderService orderService)
        {
            _dbContext = dbContext;
            _orderService = orderService;
        }

        [HttpGet("getTenOrders")]
        public IActionResult GetTenOrders()
        {
            return null;
        }

        [HttpPost("orderItem")]
        public async Task<IActionResult> OrderItem([FromBody] NewOrderDTO order)
        {
            try
            {
                var newOrder = _orderService.CreateNewOrder(order);

                await _dbContext.Orders.AddAsync(newOrder);

                await _dbContext.SaveChangesAsync();

                return Ok();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }

        }


    }
}
