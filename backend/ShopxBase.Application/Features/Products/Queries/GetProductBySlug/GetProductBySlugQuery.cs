using MediatR;
using ShopxBase.Application.DTOs.Product;

namespace ShopxBase.Application.Features.Products.Queries.GetProductBySlug;

public class GetProductBySlugQuery : IRequest<ProductDto>
{
    public string Slug { get; set; }
}
