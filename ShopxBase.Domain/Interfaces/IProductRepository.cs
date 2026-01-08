using ShopxBase.Domain.Entities;
using System.Linq.Expressions;

namespace ShopxBase.Domain.Interfaces;

/// <summary>
/// Product Repository Interface - extends generic repository with custom queries
/// </summary>
public interface IProductRepository : IRepository<Product>
{
    /// <summary>
    /// Get product by slug
    /// </summary>
    Task<Product> GetBySlugAsync(string slug);

    /// <summary>
    /// Get products by category
    /// </summary>
    Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);

    /// <summary>
    /// Get products by brand
    /// </summary>
    Task<IEnumerable<Product>> GetByBrandAsync(int brandId);

    /// <summary>
    /// Search products by name or description
    /// </summary>
    Task<IEnumerable<Product>> SearchAsync(string keyword);

    /// <summary>
    /// Get products with pagination
    /// </summary>
    Task<(IEnumerable<Product> items, int total)> GetPaginatedAsync(int pageNumber, int pageSize);

    /// <summary>
    /// Get products with filtering and pagination
    /// </summary>
    Task<(IEnumerable<Product> items, int total)> GetFilteredAsync(
        Expression<Func<Product, bool>> predicate,
        int pageNumber,
        int pageSize);

    /// <summary>
    /// Get in-stock products
    /// </summary>
    Task<IEnumerable<Product>> GetInStockAsync();

    /// <summary>
    /// Get best-selling products
    /// </summary>
    Task<IEnumerable<Product>> GetBestSellingAsync(int top = 10);

    /// <summary>
    /// Get products with ratings
    /// </summary>
    Task<Product> GetWithRatingsAsync(int id);

    /// <summary>
    /// Check if product slug exists
    /// </summary>
    Task<bool> ExistsBySlugAsync(string slug);
}
