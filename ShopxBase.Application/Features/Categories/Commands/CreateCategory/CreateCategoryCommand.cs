using MediatR;
using ShopxBase.Application.DTOs.Category;

namespace ShopxBase.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommand : IRequest<CategoryDto>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Slug { get; set; }
    public string Status { get; set; } = "Active";
}
