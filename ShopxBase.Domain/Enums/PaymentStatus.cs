namespace ShopxBase.Domain.Enums;

/// <summary>
/// Payment status enumeration
/// </summary>
public enum PaymentStatus
{
    /// <summary>Pending - Chờ thanh toán</summary>
    Pending = 0,

    /// <summary>Completed - Thanh toán thành công</summary>
    Completed = 1,

    /// <summary>Failed - Thanh toán thất bại</summary>
    Failed = 2,

    /// <summary>Refunded - Đã hoàn tiền</summary>
    Refunded = 3,

    /// <summary>Cancelled - Đã hủy</summary>
    Cancelled = 4
}
