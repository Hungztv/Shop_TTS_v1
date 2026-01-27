namespace ShopxBase.Domain.Exceptions;

/// <summary>
/// Exception thrown when a user attempts to access a resource they don't own.
/// </summary>
public class ForbiddenAccessException : DomainException
{
    private const string ErrorCode = "FORBIDDEN_ACCESS";

    public ForbiddenAccessException(string message)
        : base(message, ErrorCode)
    {
    }

    public ForbiddenAccessException(string message, Exception innerException)
        : base(message, innerException, ErrorCode)
    {
    }

    /// <summary>
    /// Creates an exception for unauthorized resource access.
    /// </summary>
    public static ForbiddenAccessException ForResource(string resourceName)
        => new ForbiddenAccessException($"Bạn không có quyền truy cập {resourceName}");

    /// <summary>
    /// Creates an exception for ownership violation.
    /// </summary>
    public static ForbiddenAccessException NotOwner()
        => new ForbiddenAccessException("Bạn không có quyền truy cập tài nguyên này");

    /// <summary>
    /// Creates an exception with a custom message.
    /// </summary>
    public static ForbiddenAccessException WithMessage(string message)
        => new ForbiddenAccessException(message);
}

/// <summary>
/// Exception thrown when a user is not authenticated.
/// </summary>
public class UnauthorizedUserException : DomainException
{
    private const string ErrorCode = "UNAUTHORIZED";

    public UnauthorizedUserException(string message)
        : base(message, ErrorCode)
    {
    }

    public UnauthorizedUserException(string message, Exception innerException)
        : base(message, innerException, ErrorCode)
    {
    }

    /// <summary>
    /// Creates an exception for unauthenticated access.
    /// </summary>
    public static UnauthorizedUserException NotAuthenticated()
        => new UnauthorizedUserException("Bạn cần đăng nhập để thực hiện thao tác này");

    /// <summary>
    /// Creates an exception for missing user ID.
    /// </summary>
    public static UnauthorizedUserException UserIdNotFound()
        => new UnauthorizedUserException("Không thể xác định người dùng từ token");
}
