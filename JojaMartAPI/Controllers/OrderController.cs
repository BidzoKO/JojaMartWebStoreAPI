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

		[HttpPost("GetEightOrders"), Authorize()]
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

		[HttpPost("OrderProducts"), Authorize()]
		public async Task<IActionResult> OrderProducts()
		{
			try
			{
				var userId = int.Parse(this.User.Claims.First(i => i.Type == "Id").Value);

				await _orderService.CreateNewOrder(userId);

				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}

		}

		[HttpPost("AddOrderToCart"), Authorize]
		public async Task<ActionResult> AddOrderToCart(CartItemDTO cartItemDto)
		{
			try
			{
				var userId = int.Parse(this.User.Claims.First(i => i.Type == "Id").Value);

				var newCartItem = await _orderService.AddItemToCart(userId, cartItemDto);

				await _dbContext.SaveChangesAsync();

				return Ok();
			}
			catch (Exception ex)
			{

				return BadRequest(ex);
			}
		}

		[HttpGet("GetCartItems"), Authorize]
		public async Task<ActionResult<List<GetCartItemDTO>>> GetCartItems()
		{
			try
			{
				var userId = int.Parse(this.User.Claims.First(i => i.Type == "Id").Value);

				var cartItemList = await _orderService.CreateCartItemDtos(userId);

				if (cartItemList == null) return NoContent();

				return Ok(cartItemList);
			}
			catch (Exception ex)
			{

				return BadRequest(ex);
			}

		}

		[HttpDelete("DeleteCartItem"), Authorize]
		public async Task<ActionResult> DeleteCartItem([FromBody] GenericIntDTO itemId)
		{
			try
			{
				var userId = int.Parse(this.User.Claims.First(i => i.Type == "Id").Value);

				var result = await _dbContext.OrdersCarts.FirstOrDefaultAsync(c => c.Id == itemId.IntValue && c.UserId == userId);

				_dbContext.OrdersCarts.Remove(result);

				await _dbContext.SaveChangesAsync();

				return Ok();
			}
			catch (Exception ex)
			{

				return BadRequest(ex);
			}

		}

	}
}
