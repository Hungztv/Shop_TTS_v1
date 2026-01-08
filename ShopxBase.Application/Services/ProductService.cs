namespace ShopxBase.Application.Services;

using ShopxBase.Application.DTOs;
using ShopxBase.Application.Interfaces;
using ShopxBase.Domain.Entities;
using ShopxBase.Domain.Exceptions;
using ShopxBase.Domain.Interfaces;

/// <summary>
/// Product service implementation
/// </summary>
public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ProductDto> GetProductByIdAsync(int id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null)
            throw new EntityNotFoundException(nameof(Product), id);

        return MapToDto(product);
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        var products = await _unitOfWork.Products.GetAllAsync();
        return products.Select(MapToDto);
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
    {
        var product = new Product(
            createProductDto.Name,
            createProductDto.Description,
            createProductDto.Price,
            createProductDto.Quantity,
            createProductDto.Sku,
            createProductDto.Category
        );

        var createdProduct = await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(createdProduct);
    }

    public async Task<ProductDto> UpdateProductAsync(UpdateProductDto updateProductDto)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(updateProductDto.Id);
        if (product == null)
            throw new EntityNotFoundException(nameof(Product), updateProductDto.Id);

        product.Name = updateProductDto.Name;
        product.Description = updateProductDto.Description;
        product.Price = updateProductDto.Price;
        product.Quantity = updateProductDto.Quantity;
        product.Category = updateProductDto.Category;
        product.UpdatedAt = DateTime.UtcNow;

        var updatedProduct = await _unitOfWork.Products.UpdateAsync(product);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(updatedProduct);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        return await _unitOfWork.Products.DeleteAsync(id);
    }

    public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(string category)
    {
        var products = await _unitOfWork.Products.GetAllAsync();
        return products
            .Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
            .Select(MapToDto);
    }

    private static ProductDto MapToDto(Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Quantity = product.Quantity,
            Sku = product.Sku,
            Category = product.Category,
            IsAvailable = product.IsAvailable,
            CreatedAt = product.CreatedAt
        };
    }
}
