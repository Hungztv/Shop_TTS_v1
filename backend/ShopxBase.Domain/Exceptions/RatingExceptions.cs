namespace ShopxBase.Domain.Exceptions;

/// <summary>
/// Exception thrown when a rating is not found
/// </summary>
public class RatingNotFoundException : DomainException
{
    public RatingNotFoundException(string message) : base(message) { }

    public static RatingNotFoundException WithMessage(string message)
        => new RatingNotFoundException(message);

    public static RatingNotFoundException NotFound(int id)
        => new RatingNotFoundException($"Rating with ID {id} not found");
}

/// <summary>
/// Exception thrown when user already rated the product
/// </summary>
public class RatingDuplicateException : DomainException
{
    public RatingDuplicateException(string message) : base(message) { }

    public static RatingDuplicateException AlreadyRated(int productId)
        => new RatingDuplicateException($"You have already rated product {productId}. Please update your existing rating.");
}

/// <summary>
/// Exception thrown for unauthorized rating access
/// </summary>
public class RatingUnauthorizedException : DomainException
{
    public RatingUnauthorizedException(string message) : base(message) { }

    public static RatingUnauthorizedException UnauthorizedAccess()
        => new RatingUnauthorizedException("You are not authorized to modify this rating");
}

/// <summary>
/// Exception thrown for invalid rating data
/// </summary>
public class InvalidRatingException : DomainException
{
    public InvalidRatingException(string message) : base(message) { }

    public static InvalidRatingException InvalidScore()
        => new InvalidRatingException("Rating score must be between 1 and 5");

    public static InvalidRatingException CommentTooShort()
        => new InvalidRatingException("Comment must be at least 4 characters");
}
