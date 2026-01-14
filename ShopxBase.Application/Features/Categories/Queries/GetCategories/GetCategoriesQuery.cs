using MediatR;
using ShopxBase.Application.DTOs.Category;

namespace ShopxBase.Application.Features.Categories.Queries.GetCategories;

public class GetCategoriesQuery : IRequest<IEnumerable<CategoryDto>>
{
    // TODO: Add properties
}
