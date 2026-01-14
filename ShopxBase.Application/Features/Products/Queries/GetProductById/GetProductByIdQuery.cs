using MediatR;
using ShopxBase.Application.DTOs.Product;

namespace ShopxBase.Application.Features.Products.Queries.GetProductById;

public class GetProductByIdQuery : IRequest<ProductDto>
{
    public int Id { get; set; }
}
