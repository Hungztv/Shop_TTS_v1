using ShopxBase.Domain.Entities;
using System.Linq.Expressions;

namespace ShopxBase.Domain.Interfaces;

/// <summary>
/// Order Repository Interface - extends generic repository with custom queries
/// </summary>
public interface IOrderRepository : IRepository<Order>
{
    /// <summary>
    /// Get order by order code
    /// </summary>
    Task<Order> GetByCodeAsync(string orderCode);

    /// <summary>
    /// Get orders by user
    /// </summary>
    Task<IEnumerable<Order>> GetByUserAsync(string userId);

    /// <summary>
    /// Get orders by status
    /// </summary>
    Task<IEnumerable<Order>> GetByStatusAsync(int status);

    /// <summary>
    /// Get orders within date range
    /// </summary>
    Task<IEnumerable<Order>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

    /// <summary>
    /// Get orders with details
    /// </summary>
    Task<Order> GetWithDetailsAsync(int id);

    /// <summary>
    /// Get user orders with pagination
    /// </summary>
    Task<(IEnumerable<Order> items, int total)> GetUserOrdersPaginatedAsync(
        string userId,
        int pageNumber,
        int pageSize);

    /// <summary>
    /// Get orders with filtering
    /// </summary>
    Task<(IEnumerable<Order> items, int total)> GetFilteredAsync(
        Expression<Func<Order, bool>> predicate,
        int pageNumber,
        int pageSize);

    /// <summary>
    /// Check if order code exists
    /// </summary>
    Task<bool> ExistsByCodeAsync(string orderCode);

    /// <summary>
    /// Get pending orders count
    /// </summary>
    Task<int> GetPendingCountAsync();

    /// <summary>
    /// Get total revenue by date range
    /// </summary>
    Task<decimal> GetTotalRevenueAsync(DateTime startDate, DateTime endDate);
}
