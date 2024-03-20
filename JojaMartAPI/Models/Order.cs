using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace JojaMartAPI;

[Table("orders")]
public partial class Order
{
    [Key]
    public int Id { get; set; }

    [Column("user_Id")]
    public int UserId { get; set; }

    [Column("product_Id")]
    public int ProductId { get; set; }

    [Column("quantity")]
    public int Quantity { get; set; }

    [Column("order_date", TypeName = "datetime")]
    public DateTime OrderDate { get; set; }

    [Column("status")]
    [StringLength(1)]
    [Unicode(false)]
    public string Status { get; set; } = null!;

    [Column("shipping_address")]
    [StringLength(50)]
    [Unicode(false)]
    public string ShippingAddress { get; set; } = null!;

    [Column("total_price", TypeName = "decimal(18, 2)")]
    public decimal TotalPrice { get; set; }

    [Column("tracking_number")]
    [StringLength(18)]
    [Unicode(false)]
    public string TrackingNumber { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("Orders")]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Orders")]
    public virtual User User { get; set; } = null!;
}
