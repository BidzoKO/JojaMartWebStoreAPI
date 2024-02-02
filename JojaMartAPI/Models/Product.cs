using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace JojaMartAPI;

[Table("products")]
[Index("ProductName", Name = "UC_ProductsName", IsUnique = true)]
public partial class Product
{
    [Key]
    public int Id { get; set; }

    [Column("product_name")]
    [StringLength(50)]
    [Unicode(false)]
    public string ProductName { get; set; } = null!;

    [Column("description", TypeName = "text")]
    public string? Description { get; set; }

    [Column("price")]
    public int Price { get; set; }

    [Column("stock_quantity")]
    public int StockQuantity { get; set; }

    [Column("image_URL")]
    [StringLength(255)]
    [Unicode(false)]
    public string? ImageUrl { get; set; }

    [Column("availability_status")]
    [StringLength(1)]
    [Unicode(false)]
    public string AvailabilityStatus { get; set; } = null!;

    [Column("weight", TypeName = "decimal(8, 2)")]
    public decimal? Weight { get; set; }

    [Column("rating", TypeName = "decimal(3, 2)")]
    public decimal? Rating { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("Products")]
    public virtual ICollection<ProductCategory> Categories { get; set; } = new List<ProductCategory>();
}
