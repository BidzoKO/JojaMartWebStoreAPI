using System.ComponentModel.DataAnnotations;

namespace JojaMartAPI.DTOs.OrderDtos
{
    public class NewOrderDTO
    {
        public int UserId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        [StringLength(50)]
        public string ShippingAddress { get; set; } = null!;

        public decimal TotalPrice { get; set; }

    }
}
