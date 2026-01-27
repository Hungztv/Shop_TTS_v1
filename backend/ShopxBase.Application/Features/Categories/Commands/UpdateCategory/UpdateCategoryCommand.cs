using MediatR;
using ShopxBase.Application.DTOs.Category;

namespace ShopxBase.Application.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommand : IRequest<CategoryDto>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Slug { get; set; }
    public string Status { get; set; }
}
