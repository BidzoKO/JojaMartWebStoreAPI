using JojaMartAPI.DTOs.OrderDtos;

namespace JojaMartAPI.Services.Interfaces
{
	public interface IOrderService
	{
		public Task CreateNewOrder(int userId);
		public Task<List<GetOrderDTO>> GetUserOrders(int userId, int orderRange);
		public Task<OrdersCart> AddItemToCart(int userId, CartItemDTO cartItemDto);
		public Task<List<GetCartItemDTO>> CreateCartItemDtos(int userId);




	}
}
