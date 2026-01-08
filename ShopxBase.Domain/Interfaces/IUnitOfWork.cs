namespace ShopxBase.Domain.Interfaces;

using ShopxBase.Domain.Entities;

/// <summary>
/// Unit of Work interface for managing database transactions
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Product repository
    /// </summary>
    IRepository<Product> Products { get; }

    /// <summary>
    /// Order repository
    /// </summary>
    IRepository<Order> Orders { get; }

    /// <summary>
    /// User repository
    /// </summary>
    IRepository<User> Users { get; }

    /// <summary>
    /// Save changes to database
    /// </summary>
    Task<int> SaveChangesAsync();

    /// <summary>
    /// Begin transaction
    /// </summary>
    Task BeginTransactionAsync();

    /// <summary>
    /// Commit transaction
    /// </summary>
    Task CommitAsync();

    /// <summary>
    /// Rollback transaction
    /// </summary>
    Task RollbackAsync();
}
