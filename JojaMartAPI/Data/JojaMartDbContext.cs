using Microsoft.EntityFrameworkCore;

namespace JojaMartAPI;

public partial class JojaMartDbContext : DbContext
{
	public JojaMartDbContext()
	{
	}

	public JojaMartDbContext(DbContextOptions<JojaMartDbContext> options)
		: base(options)
	{
	}

	public virtual DbSet<Order> Orders { get; set; }

	public virtual DbSet<OrdersCart> OrdersCarts { get; set; }

	public virtual DbSet<Product> Products { get; set; }

	public virtual DbSet<ProductCategory> ProductCategories { get; set; }

	public virtual DbSet<User> Users { get; set; }

	public virtual DbSet<UserRefreshToken> UserRefreshTokens { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Order>(entity =>
		{
			entity.HasOne(d => d.Product).WithMany(p => p.Orders)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_orders_products");

			entity.HasOne(d => d.User).WithMany(p => p.Orders)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_orders_users");
		});

		modelBuilder.Entity<OrdersCart>(entity =>
		{
			entity.HasOne(d => d.Product).WithMany(p => p.OrdersCarts)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_orders_cart_products");

			entity.HasOne(d => d.User).WithMany(p => p.OrdersCarts)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_orders_cart_orders_cart");
		});

		modelBuilder.Entity<Product>(entity =>
		{
			entity.HasMany(d => d.Categories).WithMany(p => p.Products)
				.UsingEntity<Dictionary<string, object>>(
					"ProductCategoryMapping",
					r => r.HasOne<ProductCategory>().WithMany()
						.HasForeignKey("CategoryId")
						.OnDelete(DeleteBehavior.ClientSetNull)
						.HasConstraintName("FK__product_c__Categ__797309D9"),
					l => l.HasOne<Product>().WithMany()
						.HasForeignKey("ProductId")
						.OnDelete(DeleteBehavior.ClientSetNull)
						.HasConstraintName("FK__product_c__Produ__787EE5A0"),
					j =>
					{
						j.HasKey("ProductId", "CategoryId").HasName("PK__product___159C556D8C3FF11E");
						j.ToTable("product_category_mapping");
					});
		});

		modelBuilder.Entity<User>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__users__3214EC272BDBC05D");

			entity.Property(e => e.RegistrationDate).HasDefaultValueSql("(getdate())");
		});

		modelBuilder.Entity<UserRefreshToken>(entity =>
		{
			entity.HasOne(d => d.User).WithMany(p => p.UserRefreshTokens)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_user_refresh_tokens_users");
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
