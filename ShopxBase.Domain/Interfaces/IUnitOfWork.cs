namespace ShopxBase.Domain.Interfaces;

using ShopxBase.Domain.Entities;

public interface IUnitOfWork : IDisposable
{
    IRepository<Product> Products { get; }
    IRepository<Category> Categories { get; }
    IRepository<Brand> Brands { get; }
    IRepository<Order> Orders { get; }
    IRepository<OrderDetail> OrderDetails { get; }
    IRepository<AppUser> Users { get; }
    IRepository<Coupon> Coupons { get; }
    IRepository<Rating> Ratings { get; }
    IRepository<Wishlist> Wishlists { get; }
    IRepository<CompareProduct> CompareProducts { get; }
    IRepository<Slider> Sliders { get; }
    IRepository<Contact> Contacts { get; }

    // Specialized repositories
    IProductRepository ProductRepository { get; }
    IOrderRepository OrderRepository { get; }
    ICouponRepository CouponRepository { get; }

    Task<int> SaveChangesAsync();
    int SaveChanges();
}