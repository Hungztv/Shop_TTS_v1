using MediatR;
using ShopxBase.Application.DTOs.Brand;
using ShopxBase.Application.DTOs.Common;

namespace ShopxBase.Application.Features.Brands.Queries.GetBrands;

public class GetBrandsQuery : IRequest<PaginatedResult<BrandDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public string? Status { get; set; }
}
