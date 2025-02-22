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
    
    public async Task<T> CreateAsync(T entity, CancellationToken ct)
    {
        var res = await _dbSet.AddAsync(entity, ct);
        return res.Entity;
    }

    public Task<T> UpdateAsync(T entity, CancellationToken ct)
    {
        var res = _dbSet.Update(entity);
        return Task.FromResult(res.Entity);
    }

    public Task<T> DeleteAsync(T entity, CancellationToken ct)
    {
        var res = _dbSet.Remove(entity);
        return Task.FromResult(res.Entity);
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _dbSet.FindAsync([id], ct);
    }


    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct)
    {
        return await _dbSet.AsNoTracking().ToListAsync(ct);
    }

    public async Task<IEnumerable<T>> GetFilteredAsync(Expression<Func<T, bool>> predicate, CancellationToken ct)
    {
        return await _dbSet.AsNoTracking().Where(predicate).ToListAsync(ct);
    }
}