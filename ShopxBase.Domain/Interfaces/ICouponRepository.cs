using ShopxBase.Domain.Entities;

namespace ShopxBase.Domain.Interfaces;

/// <summary>
/// Coupon Repository Interface - extends generic repository with custom queries
/// </summary>
public interface ICouponRepository : IRepository<Coupon>
{
    /// <summary>
    /// Get coupon by code
    /// </summary>
    Task<Coupon> GetByCodeAsync(string code);

    /// <summary>
    /// Get active coupons
    /// </summary>
    Task<IEnumerable<Coupon>> GetActiveCouponsAsync();

    /// <summary>
    /// Get expired coupons
    /// </summary>
    Task<IEnumerable<Coupon>> GetExpiredCouponsAsync();

    /// <summary>
    /// Validate coupon (check validity and availability)
    /// </summary>
    Task<bool> IsValidAsync(string code);

    /// <summary>
    /// Check if coupon code exists
    /// </summary>
    Task<bool> ExistsByCodeAsync(string code);

    /// <summary>
    /// Get available coupons (not expired, not used up)
    /// </summary>
    Task<IEnumerable<Coupon>> GetAvailableCouponsAsync();

    /// <summary>
    /// Get coupons with pagination
    /// </summary>
    Task<(IEnumerable<Coupon> items, int total)> GetPaginatedAsync(int pageNumber, int pageSize);

    /// <summary>
    /// Get coupons by discount value range
    /// </summary>
    Task<IEnumerable<Coupon>> GetByDiscountRangeAsync(decimal minValue, decimal maxValue);

    /// <summary>
    /// Get most used coupons
    /// </summary>
    Task<IEnumerable<Coupon>> GetMostUsedAsync(int top = 10);

    /// <summary>
    /// Get coupon usage statistics
    /// </summary>
    Task<(int totalIssued, int totalUsed, int available)> GetStatisticsAsync(int couponId);
}
