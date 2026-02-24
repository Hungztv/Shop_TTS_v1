namespace ShopxBase.Domain.Exceptions;

public class BusinessRegistrationNotFoundException : DomainException
{
    private const string ErrorCode = "BUSINESS_REGISTRATION_NOT_FOUND";

    public BusinessRegistrationNotFoundException(string message)
        : base(message, ErrorCode)
    {
    }
}

public class ShopNotFoundException : DomainException
{
    private const string ErrorCode = "SHOP_NOT_FOUND";

    public ShopNotFoundException(string message)
        : base(message, ErrorCode)
    {
    }
}

public class DuplicateRegistrationException : DomainException
{
    private const string ErrorCode = "DUPLICATE_REGISTRATION";

    public DuplicateRegistrationException(string message)
        : base(message, ErrorCode)
    {
    }
}

public class DuplicateShopSlugException : DomainException
{
    private const string ErrorCode = "DUPLICATE_SHOP_SLUG";

    public DuplicateShopSlugException(string message)
        : base(message, ErrorCode)
    {
    }
}
