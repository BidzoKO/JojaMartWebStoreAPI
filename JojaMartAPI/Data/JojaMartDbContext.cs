using Microsoft.EntityFrameworkCore;


public partial class JojaMartDbContext : DbContext
{
    public JojaMartDbContext()
    {
    }

    public JojaMartDbContext(DbContextOptions<JojaMartDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRefreshToken> UserRefreshTokens { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3214EC272BDBC05D");

            entity.Property(e => e.RegistrationDate).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<UserRefreshToken>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.UserRefreshToken)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_user_refresh_tokens_users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
