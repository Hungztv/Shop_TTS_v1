using MediatR;
using ShopxBase.Domain.Interfaces;

namespace ShopxBase.Application.Features.Coupons.Queries.ValidateCoupon;

public class ValidateCouponQueryHandler : IRequestHandler<ValidateCouponQuery, ValidateCouponResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public ValidateCouponQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ValidateCouponResult> Handle(ValidateCouponQuery request, CancellationToken cancellationToken)
    {
        var coupon = await _unitOfWork.Coupons.FirstOrDefaultAsync(c => c.Code == request.Code);

        if (coupon == null)
        {
            return new ValidateCouponResult
            {
                IsValid = false,
                Message = "Mã coupon không tồn tại",
                DiscountAmount = 0,
                FinalAmount = request.OrderValue
            };
        }

        if (!coupon.IsValid())
        {
            var reason = coupon.Status == 0 ? "Coupon chưa được kích hoạt" :
                        coupon.IsExpired() ? "Coupon đã hết hạn" :
                        coupon.IsOutOfStock() ? "Coupon đã hết lượt sử dụng" :
                        DateTime.Now < coupon.DateStart ? "Coupon chưa đến thời gian sử dụng" :
                        "Coupon không hợp lệ";

            return new ValidateCouponResult
            {
                IsValid = false,
                Message = reason,
                DiscountAmount = 0,
                FinalAmount = request.OrderValue
            };
        }

        if (request.OrderValue < coupon.MinimumOrderValue)
        {
            return new ValidateCouponResult
            {
                IsValid = false,
                Message = $"Đơn hàng tối thiểu {coupon.MinimumOrderValue:N0} VNĐ để sử dụng coupon này",
                DiscountAmount = 0,
                FinalAmount = request.OrderValue
            };
        }

        var discountAmount = coupon.CalculateDiscount(request.OrderValue);
        var finalAmount = request.OrderValue - discountAmount;

        return new ValidateCouponResult
        {
            IsValid = true,
            Message = coupon.IsPercent
                ? $"Giảm {coupon.DiscountValue}% - Tiết kiệm {discountAmount:N0} VNĐ"
                : $"Giảm {discountAmount:N0} VNĐ",
            DiscountAmount = discountAmount,
            FinalAmount = finalAmount
        };
    }
}
