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
    public class UserRepository : IUserRepository
    {
        protected readonly DbContext _context;
        protected readonly DbSet<AppUser> _dbSet;

        public UserRepository(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<AppUser>();
        }

        // READ operations
        public async Task<AppUser> GetByIdAsync(string userId)
        {
            return await _dbSet.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<AppUser> GetByEmailAsync(string email)
        {
            return await _dbSet.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<AppUser> GetByUserNameAsync(string userName)
        {
            return await _dbSet.AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task<IEnumerable<AppUser>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<AppUser>> FindAsync(Expression<Func<AppUser, bool>> predicate)
        {
            return await _dbSet.AsNoTracking()
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<AppUser> FirstOrDefaultAsync(Expression<Func<AppUser, bool>> predicate)
        {
            return await _dbSet.AsNoTracking()
                .FirstOrDefaultAsync(predicate);
        }

        // CREATE
        public async Task<AppUser> AddAsync(AppUser entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        // UPDATE
        public async Task<AppUser> UpdateAsync(AppUser entity)
        {
            _dbSet.Update(entity);
            return await Task.FromResult(entity);
        }

        // DELETE
        public async Task<bool> DeleteAsync(string userId)
        {
            var user = await _dbSet.FindAsync(userId);
            if (user == null) return false;

            _dbSet.Remove(user);
            return true;
        }

        // UTILITY
        public async Task<bool> ExistsAsync(string userId)
        {
            return await _dbSet.AnyAsync(u => u.Id == userId);
        }

        public async Task<int> CountAsync(Expression<Func<AppUser, bool>>? predicate = null)
        {
            if (predicate == null)
            {
                return await _dbSet.CountAsync();
            }
            return await _dbSet.CountAsync(predicate);
        }

        public async Task<bool> AnyAsync(Expression<Func<AppUser, bool>>? predicate = null)
        {
            if (predicate == null)
            {
                return await _dbSet.AnyAsync();
            }
            return await _dbSet.AnyAsync(predicate);
        }
    }
}
