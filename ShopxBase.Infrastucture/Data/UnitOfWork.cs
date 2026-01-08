namespace ShopxBase.Infrastructure.Data;

using ShopxBase.Domain.Entities;
using ShopxBase.Domain.Interfaces;
using ShopxBase.Infrastructure.Data.DbContext;
using ShopxBase.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

/// <summary>
/// Unit of Work implementation
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ShoppingDbContext _context;
    private IDbContextTransaction _transaction;

    private IRepository<Product> _productRepository;
    private IRepository<Order> _orderRepository;
    private IRepository<User> _userRepository;

    public UnitOfWork(ShoppingDbContext context)
    {
        _context = context;
    }

    public IRepository<Product> Products =>
        _productRepository ??= new ProductRepository(_context);

    public IRepository<Order> Orders =>
        _orderRepository ??= new Repository<Order>(_context);

    public IRepository<User> Users =>
        _userRepository ??= new Repository<User>(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        try
        {
            await SaveChangesAsync();
            await _transaction?.CommitAsync();
        }
        catch
        {
            await RollbackAsync();
            throw;
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;
        }
    }

    public async Task RollbackAsync()
    {
        try
        {
            await _transaction?.RollbackAsync();
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context?.Dispose();
    }
}
