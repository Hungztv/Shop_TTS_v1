namespace ShopxBase.Domain.Exceptions;

public class InvalidCouponException : DomainException
{
    private const string ErrorCode = "INVALID_COUPON";

    public InvalidCouponException(string message)
        : base(message, ErrorCode)
    {
    }

    private InvalidCouponException(string message, bool _)
        : base(message, ErrorCode)
    {
    }

    public static InvalidCouponException NotFound(string code)
        => new InvalidCouponException($"Mã giảm giá '{code}' không tồn tại", true);

    public static InvalidCouponException Expired(string code)
        => new InvalidCouponException($"Mã giảm giá '{code}' đã hết hạn", true);

    public static InvalidCouponException OutOfStock(string code)
        => new InvalidCouponException($"Mã giảm giá '{code}' đã hết lượt sử dụng", true);

    public static InvalidCouponException MinimumOrderNotMet(string code, decimal minimum, decimal current)
        => new InvalidCouponException(
            $"Mã giảm giá '{code}' yêu cầu đơn hàng tối thiểu {minimum:N0}đ, hiện tại: {current:N0}đ", true);

    public static InvalidCouponException Inactive(string code)
        => new InvalidCouponException($"Mã giảm giá '{code}' không còn hoạt động", true);
}