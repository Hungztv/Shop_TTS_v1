namespace ShopxBase.Domain.Enums;

public enum OrderStatus
{
    Pending = 0,      // Chờ xử lý
    Confirmed = 1,    // Đã xác nhận
    Shipping = 2,     // Đang giao
    Completed = 3,    // Đã giao
    Cancelled = 4,    // Đã hủy
    Refunded = 5      // Đã hoàn tiền (nếu áp dụng)
}
