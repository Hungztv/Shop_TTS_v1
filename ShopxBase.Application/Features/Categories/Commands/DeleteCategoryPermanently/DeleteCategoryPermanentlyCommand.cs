using MediatR;

namespace ShopxBase.Application.Features.Categories.Commands.DeleteCategoryPermanently;

public class DeleteCategoryPermanentlyCommand : IRequest<bool>
{
    public int Id { get; set; }
}
