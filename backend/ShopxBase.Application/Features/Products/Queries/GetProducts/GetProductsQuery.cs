using MediatR;
using ShopxBase.Application.DTOs.Product;
using ShopxBase.Application.DTOs.Common;

namespace ShopxBase.Application.Features.Products.Queries.GetProducts;

public class GetProductsQuery : IRequest<PaginationResponse<ProductDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
