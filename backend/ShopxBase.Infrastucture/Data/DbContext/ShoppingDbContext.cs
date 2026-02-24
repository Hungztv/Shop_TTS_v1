using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopxBase.Domain.Entities;
using System.Reflection;

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
        public DbSet<Cart> Carts { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<BusinessRegistration> BusinessRegistrations { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<ShopMember> ShopMembers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShopxBaseDbContext).Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            NormalizeDateTimes();
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            NormalizeDateTimes();
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

        private void NormalizeDateTimes()
        {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (entry.State != EntityState.Added && entry.State != EntityState.Modified)
                    continue;

                var entity = entry.Entity;
                if (entity == null)
                    continue;

                var props = entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var prop in props)
                {
                    if (prop.PropertyType == typeof(DateTime))
                    {
                        var value = (DateTime)prop.GetValue(entity)!;
                        if (value.Kind == DateTimeKind.Unspecified)
                            prop.SetValue(entity, DateTime.SpecifyKind(value, DateTimeKind.Utc));
                        else if (value.Kind == DateTimeKind.Local)
                            prop.SetValue(entity, value.ToUniversalTime());
                    }
                    else if (prop.PropertyType == typeof(DateTime?))
                    {
                        var value = (DateTime?)prop.GetValue(entity);
                        if (value.HasValue)
                        {
                            var dt = value.Value;
                            if (dt.Kind == DateTimeKind.Unspecified)
                                dt = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
                            else if (dt.Kind == DateTimeKind.Local)
                                dt = dt.ToUniversalTime();
                            prop.SetValue(entity, dt);
                        }
                    }
                }
            }
        }
    }
}