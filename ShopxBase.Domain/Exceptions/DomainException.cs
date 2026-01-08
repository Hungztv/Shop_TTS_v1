namespace ShopxBase.Domain.Exceptions;

/// <summary>
/// Base exception for domain layer
/// </summary>
public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }

    public DomainException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

/// <summary>
/// Exception thrown when a domain entity is not found
/// </summary>
public class EntityNotFoundException : DomainException
{
    public EntityNotFoundException(string entityName, int id)
        : base($"{entityName} with ID {id} was not found.")
    {
    }

    public EntityNotFoundException(string message) : base(message)
    {
    }
}

/// <summary>
/// Exception thrown for invalid domain operations
/// </summary>
public class InvalidDomainOperationException : DomainException
{
    public InvalidDomainOperationException(string message) : base(message)
    {
    }
}
