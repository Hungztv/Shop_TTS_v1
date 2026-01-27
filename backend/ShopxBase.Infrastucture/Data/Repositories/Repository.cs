using System;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ShopxBase.Domain.Entities;
using ShopxBase.Domain.Interfaces;

namespace ShopxBase.Infrastructure.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }



        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking()
                .Where(e => !e.IsDeleted)
                .ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AsNoTracking()
                .Where(e => !e.IsDeleted)
                .Where(predicate)
                .ToListAsync();
        }

        public virtual async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AsNoTracking()
                .Where(e => !e.IsDeleted)
                .FirstOrDefaultAsync(predicate);
        }

        // CREATE

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }


        public virtual async Task<T> AddRangeAsync(IEnumerable<T> entities)
        {
            var list = entities as IList<T> ?? entities.ToList();
            if (!list.Any())
                return default!;

            await _dbSet.AddRangeAsync(list);
            return list.First();
        }



        public virtual async Task<T> UpdateAsync(T entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            _dbSet.Update(entity);
            return await Task.FromResult(entity);
        }



        public virtual async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
            if (entity == null)
                return false;

            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
            _dbSet.Update(entity);
            return true;
        }

        public virtual async Task<bool> DeletePermanentlyAsync(int id)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
            if (entity == null)
                return false;

            _dbSet.Remove(entity); // Hard delete - xóa vĩnh viễn
            return true;
        }


        public virtual async Task<bool> ExistsAsync(int id)
        {
            return await _dbSet.AnyAsync(e => e.Id == id && !e.IsDeleted);
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            var query = _dbSet.Where(e => !e.IsDeleted).AsQueryable();
            if (predicate != null)
                query = query.Where(predicate);
            return await query.CountAsync();
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>>? predicate = null)
        {
            var query = _dbSet.Where(e => !e.IsDeleted).AsQueryable();
            if (predicate != null)
                query = query.Where(predicate);
            return await query.AnyAsync();
        }
    }
}