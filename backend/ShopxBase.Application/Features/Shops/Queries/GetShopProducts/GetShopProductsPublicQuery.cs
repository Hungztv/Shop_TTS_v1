using MediatR;
using ShopxBase.Application.DTOs.Product;
using ShopxBase.Application.DTOs.Common;

namespace ShopxBase.Application.Features.Shops.Queries.GetShopProducts;

public class GetShopProductsPublicQuery : IRequest<PaginationResponse<ProductDto>>
{
    public int ShopId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 12;
    public int? CategoryId { get; set; }
    public string? Search { get; set; }
    public string? SortBy { get; set; }      // price, createdAt, soldOut
    public string? SortOrder { get; set; }   // asc, desc
}
