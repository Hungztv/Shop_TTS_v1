namespace ShopxBase.Infrastructure.Data.DbContext;

using Microsoft.EntityFrameworkCore;
using ShopxBase.Domain.Entities;

/// <summary>
/// Shopping database context
/// </summary>
public class ShoppingDbContext : DbContext
{
    public ShoppingDbContext(DbContextOptions<ShoppingDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Products table
    /// </summary>
    public DbSet<Product> Products { get; set; }

    /// <summary>
    /// Users table
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Orders table
    /// </summary>
    public DbSet<Order> Orders { get; set; }

    /// <summary>
    /// Order Items table
    /// </summary>
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Product entity
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(256);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Price).HasPrecision(18, 2);
            entity.Property(e => e.Sku).HasMaxLength(50);
            entity.Property(e => e.Category).HasMaxLength(100);
        });

        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
            entity.Property(e => e.FullName).HasMaxLength(256);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
        });

        // Configure Order entity
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
            entity.Property(e => e.ShippingAddress).HasMaxLength(500);
            entity.Property(e => e.Notes).HasMaxLength(1000);

            // Configure relationships
            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId);

            entity.HasMany(e => e.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId);
        });

        // Configure OrderItem entity
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
            entity.Property(e => e.TotalPrice).HasPrecision(18, 2);

            // Configure relationships
            entity.HasOne(e => e.Product)
                .WithMany()
                .HasForeignKey(e => e.ProductId);
        });
    }
}
