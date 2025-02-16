using System.Linq.Expressions;

namespace Backend.Core.Database.Repositories.Base;

public interface ICrudRepository<T> where T : class
{
    Task<T> CreateAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<T> DeleteAsync(T entity);
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> GetFilteredAsync(Expression<Func<T, bool>> predicate);
}