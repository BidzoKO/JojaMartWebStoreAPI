using JojaMartAPI.DTOs.GenericDtos;
using JojaMartAPI.DTOs.OrderDtos;
using JojaMartAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

		[HttpPost("getEightOrders", Name = "getEightOrders"), Authorize()]
		public async Task<ActionResult<List<GetOrderDTO>>> GetEightOrders([FromBody] GenericIntDTO orderRange)
		{
			try
			{
				var userId = int.Parse(this.User.Claims.First(i => i.Type == "Id").Value);

				var orders = await _orderService.GetUserOrders(userId, orderRange.IntValue);

				return Ok(orders);
			}
			catch (Exception ex)
			{

				return BadRequest(ex.Message);
			}
		}

		[HttpPost("orderProduct", Name = "orderProduct"), Authorize()]
		public async Task<IActionResult> OrderProduct([FromBody] NewOrderDTO order)
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
