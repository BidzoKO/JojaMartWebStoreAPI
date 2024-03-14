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

		public async Task CreateNewOrder(int userId)
		{
			var cartItems = await _dbContext.OrdersCarts.Where(c => c.UserId == userId).ToListAsync();

			if (cartItems == null || cartItems.Count == 0)
			{
				return;
			}

			var user = await _dbContext.Users.FirstOrDefaultAsync(c => c.Id == userId);

			var random = new Random();
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			var randString = string.Empty;

			if (user.Address == null)
			{
				user.Address = "your home";
			}

			var orderItems = new List<Order>();
			foreach (var item in cartItems)
			{
				randString = new string(Enumerable.Repeat(chars, 18)
				.Select(s => s[random.Next(s.Length)]).ToArray());

				orderItems.Add(new Order
				{
					UserId = userId,
					ProductId = item.ProductId,
					Quantity = item.Quantity,
					OrderDate = DateTime.UtcNow,
					Status = "p",
					ShippingAddress = user.Address,
					TotalPrice = item.TotalPrice,
					TrackingNumber = randString
				});
			}

			await using var transaction = await _dbContext.Database.BeginTransactionAsync();


			_dbContext.OrdersCarts.RemoveRange(cartItems);
			await _dbContext.Orders.AddRangeAsync(orderItems);
			await _dbContext.SaveChangesAsync();
			await transaction.CommitAsync();
		}

		public async Task<List<GetOrderDTO>> GetUserOrders(int userId, int orderRange)
		{
			var orders = await _dbContext.Orders.Where(c => c.UserId == userId).OrderByDescending(c => c.Id).Skip(orderRange).Take(8).ToListAsync();

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

		public async Task<OrdersCart> AddItemToCart(int userId, CartItemDTO cartItemDto)
		{
			var productObject = _dbContext.Products.FirstOrDefault(c => c.ProductName == cartItemDto.ProductName);

			if (productObject == null)
			{
				throw new ArgumentNullException(nameof(productObject));
			}

			var orderTotal = cartItemDto.Quantity * productObject.Price;

			var cartItem = new OrdersCart()
			{
				UserId = userId,
				ProductId = productObject.Id,
				Quantity = cartItemDto.Quantity,
				TotalPrice = orderTotal,
			};

			var result = await _dbContext.OrdersCarts.AddAsync(cartItem);
			await _dbContext.SaveChangesAsync();

			return cartItem;
		}

		public async Task<List<GetCartItemDTO>> CreateCartItemDtos(int userId)
		{
			var cartItems = await _dbContext.VGetCartItems.Where(c => c.UserId == userId).ToListAsync();

			if (cartItems.Count == 0) return null;

			var cartItemDtos = new List<GetCartItemDTO>();

			foreach (var cartItem in cartItems)
			{
				if (cartItem.ImageUrl == null) throw new ArgumentNullException(nameof(cartItem.ImageUrl));

				cartItemDtos.Add(new GetCartItemDTO()
				{
					Id = cartItem.Id,
					ImageUrl = cartItem.ImageUrl,
					ProductName = cartItem.ProductName,
					Quantity = cartItem.Quantity,
					TotalPrice = cartItem.TotalPrice,
				});
			}

			return cartItemDtos;
		}
	}
}
