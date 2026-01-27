using MediatR;

namespace ShopxBase.Application.Features.Brands.Commands.DeleteBrand;

public class DeleteBrandCommand : IRequest<bool>
{
    public int Id { get; set; }
}
