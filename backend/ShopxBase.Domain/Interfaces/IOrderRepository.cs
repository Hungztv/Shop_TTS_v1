using ShopxBase.Domain.Entities;
using System.Linq.Expressions;

namespace ShopxBase.Domain.Interfaces;


public interface IOrderRepository : IRepository<Order>
{

    Task<Order> GetByCodeAsync(string orderCode);

    Task<IEnumerable<Order>> GetByUserAsync(string userId);

    Task<IEnumerable<Order>> GetByStatusAsync(int status);

    Task<IEnumerable<Order>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

    Task<Order> GetWithDetailsAsync(int id);

    Task<(IEnumerable<Order> items, int total)> GetUserOrdersPaginatedAsync(
        string userId,
        int pageNumber,
        int pageSize);

    Task<(IEnumerable<Order> items, int total)> GetFilteredAsync(
        Expression<Func<Order, bool>> predicate,
        int pageNumber,
        int pageSize);

    Task<bool> ExistsByCodeAsync(string orderCode);

    Task<int> GetPendingCountAsync();

    Task<decimal> GetTotalRevenueAsync(DateTime startDate, DateTime endDate);
}
