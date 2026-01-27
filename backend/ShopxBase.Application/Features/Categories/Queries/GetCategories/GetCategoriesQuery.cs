using MediatR;
using ShopxBase.Application.DTOs.Category;
using ShopxBase.Application.DTOs.Common;

namespace ShopxBase.Application.Features.Categories.Queries.GetCategories;

public class GetCategoriesQuery : IRequest<PaginatedResult<CategoryDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public string? Status { get; set; }
}
