namespace ShopxBase.Domain.Exceptions;

/// <summary>
/// Exception thrown when a compare item is not found
/// </summary>
public class CompareNotFoundException : DomainException
{
    public CompareNotFoundException(string message) : base(message) { }

    public static CompareNotFoundException WithMessage(string message)
        => new CompareNotFoundException(message);

    public static CompareNotFoundException NotFound(int id)
        => new CompareNotFoundException($"Compare item with ID {id} not found");
}

/// <summary>
/// Exception thrown when compare item already exists
/// </summary>
public class CompareDuplicateException : DomainException
{
    public CompareDuplicateException(string message) : base(message) { }

    public static CompareDuplicateException AlreadyExists(int productId)
        => new CompareDuplicateException($"Product {productId} is already in your compare list");
}

/// <summary>
/// Exception thrown when compare list is full (max 5 items)
/// </summary>
public class CompareListFullException : DomainException
{
    public CompareListFullException(string message) : base(message) { }

    public static CompareListFullException MaxItemsReached()
        => new CompareListFullException("Compare list is full. Maximum 5 products allowed. Please remove an item before adding a new one.");
}

/// <summary>
/// Exception thrown for unauthorized compare access
/// </summary>
public class CompareUnauthorizedException : DomainException
{
    public CompareUnauthorizedException(string message) : base(message) { }

    public static CompareUnauthorizedException UnauthorizedAccess()
        => new CompareUnauthorizedException("You are not authorized to access this compare item");
}
