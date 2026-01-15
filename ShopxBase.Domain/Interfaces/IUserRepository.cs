using ShopxBase.Domain.Entities;
using System.Linq.Expressions;

namespace ShopxBase.Domain.Interfaces;

public interface IUserRepository
{
    Task<AppUser> GetByIdAsync(string userId);
    Task<AppUser> GetByEmailAsync(string email);
    Task<AppUser> GetByUserNameAsync(string userName);
    Task<IEnumerable<AppUser>> GetAllAsync();
    Task<IEnumerable<AppUser>> FindAsync(Expression<Func<AppUser, bool>> predicate);
    Task<AppUser> FirstOrDefaultAsync(Expression<Func<AppUser, bool>> predicate);
    Task<AppUser> AddAsync(AppUser entity);
    Task<AppUser> UpdateAsync(AppUser entity);
    Task<bool> DeleteAsync(string userId);
    Task<bool> ExistsAsync(string userId);
    Task<int> CountAsync(Expression<Func<AppUser, bool>>? predicate = null);
    Task<bool> AnyAsync(Expression<Func<AppUser, bool>>? predicate = null);
}
