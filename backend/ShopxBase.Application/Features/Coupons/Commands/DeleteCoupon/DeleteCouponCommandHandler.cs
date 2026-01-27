using MediatR;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Domain.Exceptions;

namespace ShopxBase.Application.Features.Coupons.Commands.DeleteCoupon;

public class DeleteCouponCommandHandler : IRequestHandler<DeleteCouponCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCouponCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteCouponCommand request, CancellationToken cancellationToken)
    {
        var coupon = await _unitOfWork.Coupons.GetByIdAsync(request.Id);
        if (coupon == null)
            throw new InvalidCouponException($"Coupon với Id {request.Id} không tồn tại");

        // Check if coupon has been used
        if (coupon.UsedCount > 0)
            throw new DomainException($"Không thể xóa coupon đã được sử dụng ({coupon.UsedCount} lần)");

        await _unitOfWork.Coupons.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
