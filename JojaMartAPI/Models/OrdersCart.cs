using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace JojaMartAPI;

[Table("orders_cart")]
public partial class OrdersCart
{
    [Key]
    public int Id { get; set; }

    [Column("user_Id")]
    public int UserId { get; set; }

    [Column("product_Id")]
    public int ProductId { get; set; }

    [Column("quantity")]
    public int Quantity { get; set; }

    [Column("total_price", TypeName = "decimal(18, 2)")]
    public decimal TotalPrice { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("OrdersCarts")]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("OrdersCarts")]
    public virtual User User { get; set; } = null!;
}
