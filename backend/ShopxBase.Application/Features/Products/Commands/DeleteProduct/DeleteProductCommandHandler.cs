using MediatR;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Domain.Exceptions;

namespace ShopxBase.Application.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        // 1. Check product exists
        var product = await _unitOfWork.Products.GetByIdAsync(request.Id);
        if (product == null)
            throw new InvalidProductException($"Sản phẩm với Id {request.Id} không tồn tại");

        // 2. Delete (soft delete)
        var result = await _unitOfWork.Products.DeleteAsync(request.Id);

        // 3. Save changes
        if (result)
            await _unitOfWork.SaveChangesAsync();

        return result;
    }
}
