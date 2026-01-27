using MediatR;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Domain.Exceptions;

namespace ShopxBase.Application.Features.Brands.Commands.DeleteBrand;

public class DeleteBrandCommandHandler : IRequestHandler<DeleteBrandCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBrandCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
    {
        var brand = await _unitOfWork.Brands.GetByIdAsync(request.Id);
        if (brand == null)
            throw new BrandNotFoundException($"Thương hiệu với Id {request.Id} không tồn tại");

        // Check if brand has products
        var hasProducts = await _unitOfWork.Products.AnyAsync(p => p.BrandId == request.Id);
        if (hasProducts)
            throw new DomainException("Không thể xóa thương hiệu đang có sản phẩm");

        await _unitOfWork.Brands.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
