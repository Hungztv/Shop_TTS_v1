using MediatR;

namespace ShopxBase.Application.Features.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommand : IRequest<bool>
{
    public int Id { get; set; }
}
