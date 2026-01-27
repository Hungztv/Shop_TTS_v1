using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopxBase.Domain.Entities;
using ShopxBase.Domain.Interfaces;

namespace ShopxBase.Infrastructure.Data.Repositories
{
    public class CouponRepository : Repository<Coupon>, ICouponRepository
    {
        public CouponRepository(DbContext context) : base(context)
        {
        }

        public async Task<Coupon> GetByCodeAsync(string code)
        {
            return await _dbSet.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Code == code && !c.IsDeleted);
        }

        public async Task<IEnumerable<Coupon>> GetActiveCouponsAsync()
        {
            return await _dbSet.AsNoTracking()
                .Where(c => c.Status == 1 && !c.IsDeleted && DateTime.Now >= c.DateStart && DateTime.Now <= c.DateExpired)
                .ToListAsync();
        }

        public async Task<IEnumerable<Coupon>> GetExpiredCouponsAsync()
        {
            return await _dbSet.AsNoTracking()
                .Where(c => DateTime.Now > c.DateExpired && !c.IsDeleted)
                .ToListAsync();
        }

        public async Task<bool> IsValidAsync(string code)
        {
            var coupon = await GetByCodeAsync(code);
            if (coupon == null || coupon.IsDeleted)
                return false;

            return coupon.Status == 1 &&
                   coupon.Quantity > coupon.UsedCount &&
                   DateTime.Now >= coupon.DateStart &&
                   DateTime.Now <= coupon.DateExpired;
        }

        public async Task<bool> ExistsByCodeAsync(string code)
        {
            return await _dbSet.AnyAsync(c => c.Code == code && !c.IsDeleted);
        }

        public async Task<IEnumerable<Coupon>> GetAvailableCouponsAsync()
        {
            return await _dbSet.AsNoTracking()
                .Where(c => c.Status == 1 &&
                           c.Quantity > c.UsedCount &&
                           DateTime.Now >= c.DateStart &&
                           DateTime.Now <= c.DateExpired &&
                           !c.IsDeleted)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<(IEnumerable<Coupon> items, int total)> GetPaginatedAsync(int pageNumber, int pageSize)
        {
            var query = _dbSet.AsNoTracking()
                .Where(c => !c.IsDeleted)
                .OrderByDescending(c => c.CreatedAt);

            var total = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public async Task<IEnumerable<Coupon>> GetByDiscountRangeAsync(decimal minValue, decimal maxValue)
        {
            return await _dbSet.AsNoTracking()
                .Where(c => c.DiscountValue >= minValue && c.DiscountValue <= maxValue && !c.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Coupon>> GetMostUsedAsync(int top = 10)
        {
            return await _dbSet.AsNoTracking()
                .Where(c => !c.IsDeleted)
                .OrderByDescending(c => c.UsedCount)
                .Take(top)
                .ToListAsync();
        }

        public async Task<(int totalIssued, int totalUsed, int available)> GetStatisticsAsync(int couponId)
        {
            var coupon = await _dbSet.FirstOrDefaultAsync(c => c.Id == couponId && !c.IsDeleted);
            if (coupon == null)
                return (0, 0, 0);

            return (coupon.Quantity, coupon.UsedCount, coupon.Quantity - coupon.UsedCount);
        }
    }
}