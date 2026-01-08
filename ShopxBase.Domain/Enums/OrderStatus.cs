namespace ShopxBase.Domain.Enums;

/// <summary>
/// Order status enumeration
/// </summary>
public enum OrderStatus
{
    /// <summary>Pending - Chờ xử lý</summary>
    Pending = 0,

    /// <summary>Confirmed - Đã xác nhận</summary>
    Confirmed = 1,

    /// <summary>Processing - Đang xử lý</summary>
    Processing = 2,

    /// <summary>Shipped - Đã gửi</summary>
    Shipped = 3,

    /// <summary>Delivered - Đã giao</summary>
    Delivered = 4,

    /// <summary>Cancelled - Đã hủy</summary>
    Cancelled = 5,

    /// <summary>Returned - Đã trả lại</summary>
    Returned = 6
}
