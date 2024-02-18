using JojaMartAPI.DTOs.OrderDtos;
using JojaMartAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JojaMartAPI.Services
{
	public class OrderServices : IOrderService
	{
		private readonly JojaMartDbContext _dbContext;

		public OrderServices(JojaMartDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public Order CreateNewOrder(NewOrderDTO order)
		{
			var random = new Random();

			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			var randString = new string(Enumerable.Repeat(chars, 18)
				.Select(s => s[random.Next(s.Length)]).ToArray());

			var newOrder = new Order
			{
				UserId = order.UserId,
				ProductId = order.ProductId,
				Quantity = order.Quantity,
				OrderDate = DateTime.UtcNow,
				Status = "p",
				ShippingAddress = order.ShippingAddress,
				TotalPrice = order.TotalPrice,
				TrackingNumber = randString,
			};

			return newOrder;

		}


		public async Task<List<GetOrderDTO>> GetUserOrders(int userId, int orderRange)
		{
			var orders = await _dbContext.Orders.Where(c => c.UserId == userId).OrderBy(c => c.OrderDate).Skip(orderRange).Take(8).ToListAsync();

			var orderDTOs = new List<GetOrderDTO>();

			foreach (var order in orders)
			{
				var productName = await _dbContext.Products.Where(c => c.Id == order.ProductId).Select(c => c.ProductName).FirstOrDefaultAsync();

				if (productName == null)
				{
					throw new NullReferenceException("couldnt get user orders");
				}

				orderDTOs.Add(new GetOrderDTO
				{
					Id = order.Id,
					ProductName = productName,
					Quantity = order.Quantity,
					OrderDate = order.OrderDate,
					Status = order.Status,
					TotalPrice = order.TotalPrice,
					TrackingNumber = order.TrackingNumber,
				});
			}

			return orderDTOs;
		}

	}
}
