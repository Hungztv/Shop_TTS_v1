namespace ShopxBase.Application.Interfaces;

using ShopxBase.Application.DTOs;

/// <summary>
/// Product service interface
/// </summary>
public interface IProductService
{
    /// <summary>
    /// Get product by id
    /// </summary>
    Task<ProductDto> GetProductByIdAsync(int id);

    /// <summary>
    /// Get all products
    /// </summary>
    Task<IEnumerable<ProductDto>> GetAllProductsAsync();

    /// <summary>
    /// Create new product
    /// </summary>
    Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto);

    /// <summary>
    /// Update product
    /// </summary>
    Task<ProductDto> UpdateProductAsync(UpdateProductDto updateProductDto);

    /// <summary>
    /// Delete product
    /// </summary>
    Task<bool> DeleteProductAsync(int id);

    /// <summary>
    /// Get products by category
    /// </summary>
    Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(string category);
}
