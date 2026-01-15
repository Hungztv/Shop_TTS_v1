namespace ShopxBase.Domain.Exceptions;

public class InsufficientStockException : DomainException
{
    private InsufficientStockException(string message)
        : base(message)
    {
    }

    public static InsufficientStockException For(string productName, int requested, int available)
        => new InsufficientStockException($"Không đủ hàng trong kho cho sản phẩm {productName}. Yêu cầu: {requested}, Có sẵn: {available}");
    public static InsufficientStockException WithMessage(string customMessage)
       => new InsufficientStockException(customMessage);
}

