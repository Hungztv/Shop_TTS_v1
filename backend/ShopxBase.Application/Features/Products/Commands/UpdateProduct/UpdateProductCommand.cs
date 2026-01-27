using MediatR;
using ShopxBase.Application.DTOs.Product;

namespace ShopxBase.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommand : IRequest<ProductDto>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public decimal CapitalPrice { get; set; }
    public int Quantity { get; set; }
    public string Image { get; set; }
    public int BrandId { get; set; }
    public int CategoryId { get; set; }
}
