namespace ShopxBase.Domain.Interfaces;

/// <summary>
/// Generic repository interface
/// </summary>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Get entity by id
    /// </summary>
    Task<T> GetByIdAsync(int id);

    /// <summary>
    /// Get all entities
    /// </summary>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Add new entity
    /// </summary>
    Task<T> AddAsync(T entity);

    /// <summary>
    /// Update entity
    /// </summary>
    Task<T> UpdateAsync(T entity);

    /// <summary>
    /// Delete entity
    /// </summary>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// Check if entity exists
    /// </summary>
    Task<bool> ExistsAsync(int id);
}
