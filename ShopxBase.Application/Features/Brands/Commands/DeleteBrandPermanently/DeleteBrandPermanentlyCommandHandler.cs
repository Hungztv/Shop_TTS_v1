using MediatR;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Domain.Exceptions;

namespace ShopxBase.Application.Features.Brands.Commands.DeleteBrandPermanently;

public class DeleteBrandPermanentlyCommandHandler : IRequestHandler<DeleteBrandPermanentlyCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBrandPermanentlyCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteBrandPermanentlyCommand request, CancellationToken cancellationToken)
    {
        // Check if brand exists (kể cả đã soft delete)
        var brand = await _unitOfWork.Brands.FirstOrDefaultAsync(b => b.Id == request.Id);
        if (brand == null)
            throw new BrandNotFoundException($"Thương hiệu với Id {request.Id} không tồn tại");

        // Check if brand has products (bao gồm cả products đã xóa)
        var hasProducts = await _unitOfWork.Products.AnyAsync(p => p.BrandId == request.Id);
        if (hasProducts)
            throw new DomainException("Không thể xóa vĩnh viễn thương hiệu đang có sản phẩm");

        // Hard delete - xóa vĩnh viễn khỏi database
        await _unitOfWork.Brands.DeletePermanentlyAsync(request.Id);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
