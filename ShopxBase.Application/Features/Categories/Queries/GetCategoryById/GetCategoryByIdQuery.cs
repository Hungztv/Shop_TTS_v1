using MediatR;
using ShopxBase.Application.DTOs.Category;

namespace ShopxBase.Application.Features.Categories.Queries.GetCategoryById;

public class GetCategoryByIdQuery : IRequest<CategoryDto>
{
    public int Id { get; set; }
}
