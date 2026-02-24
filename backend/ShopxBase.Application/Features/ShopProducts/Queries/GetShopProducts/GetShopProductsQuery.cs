using MediatR;
using ShopxBase.Application.DTOs.Common;
using ShopxBase.Application.DTOs.Product;

namespace ShopxBase.Application.Features.ShopProducts.Queries.GetShopProducts;

public class GetShopProductsQuery : IRequest<PaginationResponse<ProductDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
