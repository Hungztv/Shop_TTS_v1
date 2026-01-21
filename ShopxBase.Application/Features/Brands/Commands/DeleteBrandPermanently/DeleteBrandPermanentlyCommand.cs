using MediatR;

namespace ShopxBase.Application.Features.Brands.Commands.DeleteBrandPermanently;

public class DeleteBrandPermanentlyCommand : IRequest<bool>
{
    public int Id { get; set; }
}
