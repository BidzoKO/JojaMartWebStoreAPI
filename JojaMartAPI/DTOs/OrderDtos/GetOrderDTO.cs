namespace JojaMartAPI.DTOs.OrderDtos
{
	public class GetOrderDTO
	{
		public int Id { get; set; }

		public string ProductName { get; set; } = null!;

		public int Quantity { get; set; }

		public DateTime OrderDate { get; set; }

		public string Status { get; set; } = null!;

		public decimal TotalPrice { get; set; }

		public string TrackingNumber { get; set; } = null!;
	}
}
