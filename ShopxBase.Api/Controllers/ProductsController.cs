namespace ShopxBase.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using ShopxBase.Application.DTOs;
using ShopxBase.Application.Interfaces;

/// <summary>
/// Products API Controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductService productService, ILogger<ProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    /// <summary>
    /// Get all products
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllProductsAsync();
        return Ok(products);
    }

    /// <summary>
    /// Get product by id
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        return Ok(product);
    }

    /// <summary>
    /// Create new product
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductDto createProductDto)
    {
        var product = await _productService.CreateProductAsync(createProductDto);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    /// <summary>
    /// Update product
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto updateProductDto)
    {
        updateProductDto.Id = id;
        var product = await _productService.UpdateProductAsync(updateProductDto);
        return Ok(product);
    }

    /// <summary>
    /// Delete product
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _productService.DeleteProductAsync(id);
        return result ? NoContent() : NotFound();
    }

    /// <summary>
    /// Get products by category
    /// </summary>
    [HttpGet("category/{category}")]
    public async Task<IActionResult> GetByCategory(string category)
    {
        var products = await _productService.GetProductsByCategoryAsync(category);
        return Ok(products);
    }
}
