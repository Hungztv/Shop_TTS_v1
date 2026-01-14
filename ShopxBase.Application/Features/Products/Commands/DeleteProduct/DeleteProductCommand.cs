using MediatR;

namespace ShopxBase.Application.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommand : IRequest<bool>
{
    public int Id { get; set; }
}
