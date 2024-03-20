using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JojaMartAPI.Models.Views;

[Keyless]
public partial class VGetCartItem
{
	public int Id { get; set; }

	[Column("user_Id")]
	public int UserId { get; set; }

	[Column("quantity")]
	public int Quantity { get; set; }

	[Column("total_price", TypeName = "decimal(18, 2)")]
	public decimal TotalPrice { get; set; }

	[Column("image_URL")]
	[StringLength(255)]
	[Unicode(false)]
	public string? ImageUrl { get; set; }

	[Column("product_name")]
	[StringLength(50)]
	[Unicode(false)]
	public string ProductName { get; set; } = null!;
}
