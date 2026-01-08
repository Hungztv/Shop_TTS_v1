namespace ShopxBase.Infrastructure.Data.Repositories;

using ShopxBase.Domain.Entities;
using ShopxBase.Infrastructure.Data.DbContext;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Product repository implementation
/// </summary>
public class ProductRepository : Repository<Product>
{
    public ProductRepository(ShoppingDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Get products by category
    /// </summary>
    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category)
    {
        return await _context.Products
            .Where(p => p.Category.ToLower() == category.ToLower() && !p.IsDeleted)
            .ToListAsync();
    }

    /// <summary>
    /// Get available products
    /// </summary>
    public async Task<IEnumerable<Product>> GetAvailableProductsAsync()
    {
        return await _context.Products
            .Where(p => p.IsAvailable && !p.IsDeleted && p.Quantity > 0)
            .ToListAsync();
    }

    /// <summary>
    /// Get product by SKU
    /// </summary>
    public async Task<Product> GetProductBySkuAsync(string sku)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Sku == sku && !p.IsDeleted);
    }
}
