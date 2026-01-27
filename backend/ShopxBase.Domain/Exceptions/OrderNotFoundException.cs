namespace ShopxBase.Domain.Exceptions;

public class OrderNotFoundException : DomainException
{
    private OrderNotFoundException(string message)
        : base(message)
    {
    }

    public static OrderNotFoundException ById(int orderId)
        => new OrderNotFoundException($"Đơn hàng với ID {orderId} không tồn tại");

    public static OrderNotFoundException ByCode(string orderCode)
        => new OrderNotFoundException($"Đơn hàng với mã {orderCode} không tồn tại");

    public static OrderNotFoundException WithMessage(string customMessage)
        => new OrderNotFoundException(customMessage);
}