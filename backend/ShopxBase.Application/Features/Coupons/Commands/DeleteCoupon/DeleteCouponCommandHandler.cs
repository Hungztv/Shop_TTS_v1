using MediatR;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Domain.Exceptions;

namespace ShopxBase.Application.Features.Coupons.Commands.DeleteCoupon;

public class DeleteCouponCommandHandler : IRequestHandler<DeleteCouponCommand, DeleteCouponResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCouponCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<DeleteCouponResult> Handle(DeleteCouponCommand request, CancellationToken cancellationToken)
    {
        var coupon = await _unitOfWork.Coupons.GetByIdAsync(request.Id);
        if (coupon == null)
            return DeleteCouponResult.Failed($"Coupon với Id {request.Id} không tồn tại");

        // Check if coupon has been used
        if (coupon.UsedCount > 0)
            return DeleteCouponResult.Failed($"Không thể xóa coupon đã được sử dụng ({coupon.UsedCount} lần)");

        await _unitOfWork.Coupons.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync();

        return DeleteCouponResult.Succeed();
    }
}

public class DeleteCouponResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }

    public static DeleteCouponResult Succeed() => new() { Success = true };
    public static DeleteCouponResult Failed(string message) => new() { Success = false, ErrorMessage = message };
}

