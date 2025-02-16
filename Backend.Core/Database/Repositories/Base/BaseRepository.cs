using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Backend.Core.Database.Repositories.Base;

public abstract class BaseRepository <T> : ICrudRepository<T> where T : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }
    
    public async Task<T> CreateAsync(T entity)
    {
        var res = await _dbSet.AddAsync(entity);
        return res.Entity;
    }

    public Task<T> UpdateAsync(T entity)
    {
        var res = _dbSet.Update(entity);
        return Task.FromResult(res.Entity);
    }

    public Task<T> DeleteAsync(T entity)
    {
        var res = _dbSet.Remove(entity);
        return Task.FromResult(res.Entity);
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<T>> GetFilteredAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
    }
}