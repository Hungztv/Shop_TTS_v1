using ShopxBase.Domain.Entities;

namespace ShopxBase.Domain.Interfaces;

public interface ICouponRepository : IRepository<Coupon>
{

    Task<Coupon> GetByCodeAsync(string code);
    Task<IEnumerable<Coupon>> GetActiveCouponsAsync();
    Task<IEnumerable<Coupon>> GetExpiredCouponsAsync();
    Task<bool> IsValidAsync(string code);
    Task<bool> ExistsByCodeAsync(string code);
    Task<IEnumerable<Coupon>> GetAvailableCouponsAsync();
    Task<(IEnumerable<Coupon> items, int total)> GetPaginatedAsync(int pageNumber, int pageSize);
    Task<IEnumerable<Coupon>> GetByDiscountRangeAsync(decimal minValue, decimal maxValue);
    Task<IEnumerable<Coupon>> GetMostUsedAsync(int top = 10);
    Task<(int totalIssued, int totalUsed, int available)> GetStatisticsAsync(int couponId);
}
