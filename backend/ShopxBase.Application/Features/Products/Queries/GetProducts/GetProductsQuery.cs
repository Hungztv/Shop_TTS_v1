using MediatR;
using ShopxBase.Application.DTOs.Product;
using ShopxBase.Application.DTOs.Common;

namespace ShopxBase.Application.Features.Products.Queries.GetProducts;

public class GetProductsQuery : IRequest<PaginationResponse<ProductDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int? CategoryId { get; set; }
    public int? BrandId { get; set; }
    public string? Search { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string? SortBy { get; set; }
    public string? SortOrder { get; set; }
}
