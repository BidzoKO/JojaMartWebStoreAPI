using System.ComponentModel.DataAnnotations;

namespace JojaMartAPI.DTOs.OrderDtos
{
	public class GetOrdersAmountDTO
	{
		[Required]
		public string AccessToken { get; set; } = null!;

		[Required]
		public int OrdersRange { get; set; }
	}
}
