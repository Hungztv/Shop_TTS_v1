using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopxBase.Domain.Entities;
using ShopxBase.Domain.Interfaces;

namespace ShopxBase.Infrastructure.Data.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(DbContext context) : base(context)
        {
        }

        public async Task<Order> GetByCodeAsync(string orderCode)
        {
            return await _dbSet.AsNoTracking()
                .FirstOrDefaultAsync(o => o.OrderCode == orderCode && !o.IsDeleted);
        }

        public async Task<IEnumerable<Order>> GetByUserAsync(string userId)
        {
            return await _dbSet.AsNoTracking()
                .Where(o => o.UserId == userId && !o.IsDeleted)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetByStatusAsync(int status)
        {
            return await _dbSet.AsNoTracking()
                .Where(o => o.Status == status && !o.IsDeleted)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet.AsNoTracking()
                .Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate && !o.IsDeleted)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<Order> GetWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(o => o.OrderDetails)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted);
        }

        public async Task<(IEnumerable<Order> items, int total)> GetUserOrdersPaginatedAsync(
            string userId,
            int pageNumber,
            int pageSize)
        {
            var query = _dbSet.AsNoTracking()
                .Where(o => o.UserId == userId && !o.IsDeleted)
                .OrderByDescending(o => o.CreatedAt);

            var total = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public async Task<(IEnumerable<Order> items, int total)> GetFilteredAsync(
            Expression<Func<Order, bool>> predicate,
            int pageNumber,
            int pageSize)
        {
            var query = _dbSet.AsNoTracking()
                .Where(o => !o.IsDeleted)
                .Where(predicate)
                .OrderByDescending(o => o.CreatedAt);

            var total = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public async Task<bool> ExistsByCodeAsync(string orderCode)
        {
            return await _dbSet.AnyAsync(o => o.OrderCode == orderCode && !o.IsDeleted);
        }

        public async Task<int> GetPendingCountAsync()
        {
            return await _dbSet.CountAsync(o => o.Status == 0 && !o.IsDeleted);
        }

        public async Task<decimal> GetTotalRevenueAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate && !o.IsDeleted)
                .SumAsync(o => o.Total);
        }

        public async Task<(IEnumerable<Order> items, int total)> GetOrdersByShopAsync(
            int shopId, int? status, string? search, int pageNumber, int pageSize)
        {
            var query = _dbSet.AsNoTracking()
                .Include(o => o.OrderDetails)
                .Where(o => !o.IsDeleted && o.OrderDetails.Any(d => d.ShopId == shopId));

            if (status.HasValue)
                query = query.Where(o => o.Status == status.Value);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(o => o.OrderCode.Contains(search) || o.Name.Contains(search));

            var total = await query.CountAsync();
            var items = await query
                .OrderByDescending(o => o.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }
    }
}