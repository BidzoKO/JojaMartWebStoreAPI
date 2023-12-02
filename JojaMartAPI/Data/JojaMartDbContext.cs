using System;
using System.Collections.Generic;
using JojaMartAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace JojaMartAPI.Data;

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3214EC272BDBC05D");

            entity.Property(e => e.RegistrationDate).HasDefaultValueSql("(getdate())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
