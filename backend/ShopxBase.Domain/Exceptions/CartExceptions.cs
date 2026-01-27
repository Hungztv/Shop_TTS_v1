namespace ShopxBase.Domain.Exceptions;


public class CartNotFoundException : DomainException
{
    private const string ErrorCode = "CART_NOT_FOUND";

    public CartNotFoundException(string message)
        : base(message, ErrorCode)
    {
    }

    public CartNotFoundException(string message, Exception innerException)
        : base(message, innerException, ErrorCode)
    {
    }


    public static CartNotFoundException WithId(int cartId)
        => new CartNotFoundException($"Giỏ hàng với Id {cartId} không tồn tại");


    public static CartNotFoundException WithMessage(string message)
        => new CartNotFoundException(message);
}


public class CartUnauthorizedException : DomainException
{
    private const string ErrorCode = "CART_UNAUTHORIZED";

    public CartUnauthorizedException(string message)
        : base(message, ErrorCode)
    {
    }

    public CartUnauthorizedException(string message, Exception innerException)
        : base(message, innerException, ErrorCode)
    {
    }

    public static CartUnauthorizedException UnauthorizedAccess()
        => new CartUnauthorizedException("Bạn không có quyền truy cập");
}

public class ProductNotFoundException : DomainException
{
    private const string ErrorCode = "PRODUCT_NOT_FOUND";

    public ProductNotFoundException(string message)
        : base(message, ErrorCode)
    {
    }

    public ProductNotFoundException(string message, Exception innerException)
        : base(message, innerException, ErrorCode)
    {
    }

    public static ProductNotFoundException WithId(int productId)
        => new ProductNotFoundException($"Sản phẩm với Id {productId} không tồn tại");

    public static ProductNotFoundException WithMessage(string message)
        => new ProductNotFoundException(message);
}
