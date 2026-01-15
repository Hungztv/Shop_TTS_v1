namespace ShopxBase.Domain.Exceptions;


public class DomainException : Exception
{
    public string Code { get; set; }

    public DomainException(string message, string code = "DOMAIN_ERROR") : base(message)
    {
        Code = code;
    }

    public DomainException(string message, Exception innerException, string code = "DOMAIN_ERROR")
        : base(message, innerException)
    {
        Code = code;
    }
}


public class InvalidProductException : DomainException
{
    private const string ErrorCode = "INVALID_PRODUCT";

    public InvalidProductException(string message)
        : base(message, ErrorCode)
    {
    }

    public InvalidProductException(string message, Exception innerException)
        : base(message, innerException, ErrorCode)
    {
    }
}


public class InvalidOrderException : DomainException
{
    private const string ErrorCode = "INVALID_ORDER";

    public InvalidOrderException(string message)
        : base(message, ErrorCode)
    {
    }

    public InvalidOrderException(string message, Exception innerException)
        : base(message, innerException, ErrorCode)
    {
    }
}


public class UserNotFoundException : DomainException
{
    private const string ErrorCode = "USER_NOT_FOUND";

    public UserNotFoundException(string message)
        : base(message, ErrorCode)
    {
    }

    public UserNotFoundException(string message, Exception innerException)
        : base(message, innerException, ErrorCode)
    {
    }
}


public class PaymentException : DomainException
{
    private const string ErrorCode = "PAYMENT_ERROR";

    public PaymentException(string message)
        : base(message, ErrorCode)
    {
    }

    public PaymentException(string message, Exception innerException)
        : base(message, innerException, ErrorCode)
    {
    }
}


public class CategoryNotFoundException : DomainException
{
    private const string ErrorCode = "CATEGORY_NOT_FOUND";

    public CategoryNotFoundException(string message)
        : base(message, ErrorCode)
    {
    }

    public CategoryNotFoundException(string message, Exception innerException)
        : base(message, innerException, ErrorCode)
    {
    }
}


public class BrandNotFoundException : DomainException
{
    private const string ErrorCode = "BRAND_NOT_FOUND";

    public BrandNotFoundException(string message)
        : base(message, ErrorCode)
    {
    }

    public BrandNotFoundException(string message, Exception innerException)
        : base(message, innerException, ErrorCode)
    {
    }
}
