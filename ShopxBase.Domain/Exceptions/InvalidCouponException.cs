namespace ShopxBase.Domain.Exceptions;

public class InvalidCouponException : DomainException
{
    private InvalidCouponException(string message)
        : base(message)
    {
    }

    public static InvalidCouponException NotFound(string code)
        => new InvalidCouponException($"Mã giảm giá '{code}' không tồn tại");

    public static InvalidCouponException Expired(string code)
        => new InvalidCouponException($"Mã giảm giá '{code}' đã hết hạn");

    public static InvalidCouponException OutOfStock(string code)
        => new InvalidCouponException($"Mã giảm giá '{code}' đã hết lượt sử dụng");

    public static InvalidCouponException MinimumOrderNotMet(string code, decimal minimum, decimal current)
        => new InvalidCouponException(
            $"Mã giảm giá '{code}' yêu cầu đơn hàng tối thiểu {minimum:N0}đ, hiện tại: {current:N0}đ");

    public static InvalidCouponException Inactive(string code)
        => new InvalidCouponException($"Mã giảm giá '{code}' không còn hoạt động");
}