using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopxBase.Domain.Entities;

namespace ShopxBase.Infrastructure.Data;

public class ShopxDbContext : IdentityDbContext<AppUser>
{
    public ShopxDbContext(DbContextOptions<ShopxDbContext> options) : base(options)
    {
    }
    //DB sets
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<Coupon> Coupons { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Wishlist> Wishlists { get; set; }
    public DbSet<CompareProduct> CompareProducts { get; set; }
    public DbSet<Slider> Sliders { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<ProductQuantity> ProductQuantities { get; set; }
    public DbSet<Shipping> Shippings { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(ShopxDbContext).Assembly);
    }
}