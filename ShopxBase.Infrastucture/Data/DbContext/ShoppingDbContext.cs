using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopxBase.Domain.Entities;

namespace ShopxBase.Infrastructure.Data
{
    public class ShopxBaseDbContext : IdentityDbContext<AppUser>
    {
        public ShopxBaseDbContext(DbContextOptions<ShopxBaseDbContext> options) : base(options)
        {
        }

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShopxBaseDbContext).Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }
        }
    }
}