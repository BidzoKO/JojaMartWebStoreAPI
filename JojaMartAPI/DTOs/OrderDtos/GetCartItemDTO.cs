namespace JojaMartAPI.DTOs.OrderDtos
{
	public class GetCartItemDTO
	{
		public int Id { get; set; }
		public string ImageUrl { get; set; } = null!;
		public string ProductName { get; set; } = null!;
		public int Quantity { get; set; }
		public decimal TotalPrice { get; set; }
	}
}
