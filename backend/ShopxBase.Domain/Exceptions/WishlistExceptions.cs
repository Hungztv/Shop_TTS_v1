namespace ShopxBase.Domain.Exceptions;

/// <summary>
/// Exception thrown when a wishlist item is not found
/// </summary>
public class WishlistNotFoundException : DomainException
{
    public WishlistNotFoundException(string message) : base(message) { }

    public static WishlistNotFoundException WithMessage(string message)
        => new WishlistNotFoundException(message);

    public static WishlistNotFoundException NotFound(int id)
        => new WishlistNotFoundException($"Wishlist item with ID {id} not found");
}

/// <summary>
/// Exception thrown when wishlist item already exists
/// </summary>
public class WishlistDuplicateException : DomainException
{
    public WishlistDuplicateException(string message) : base(message) { }

    public static WishlistDuplicateException AlreadyExists(int productId)
        => new WishlistDuplicateException($"Product {productId} is already in your wishlist");
}

/// <summary>
/// Exception thrown for unauthorized wishlist access
/// </summary>
public class WishlistUnauthorizedException : DomainException
{
    public WishlistUnauthorizedException(string message) : base(message) { }

    public static WishlistUnauthorizedException UnauthorizedAccess()
        => new WishlistUnauthorizedException("You are not authorized to access this wishlist item");
}
