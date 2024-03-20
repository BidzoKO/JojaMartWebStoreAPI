using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace JojaMartAPI;

[Table("product_categories")]
public partial class ProductCategory
{
    [Key]
    public int Id { get; set; }

    [Column("category_name")]
    [StringLength(50)]
    [Unicode(false)]
    public string? CategoryName { get; set; }

    [Column("description", TypeName = "text")]
    public string Description { get; set; } = null!;

    [ForeignKey("CategoryId")]
    [InverseProperty("Categories")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
