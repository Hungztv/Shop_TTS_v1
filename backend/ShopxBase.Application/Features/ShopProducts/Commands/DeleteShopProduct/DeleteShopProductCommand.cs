using MediatR;

namespace ShopxBase.Application.Features.ShopProducts.Commands.DeleteShopProduct;

public class DeleteShopProductCommand : IRequest<bool>
{
    public int Id { get; set; }
}
