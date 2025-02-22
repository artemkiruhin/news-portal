using System.Linq.Expressions;

namespace Backend.Core.Database.Repositories.Base;

public interface ICrudRepository<T> where T : class
{
    Task<T> CreateAsync(T entity, CancellationToken ct);

    Task<T> UpdateAsync(T entity, CancellationToken ct);

    Task<T> DeleteAsync(T entity, CancellationToken ct);

    Task<T?> GetByIdAsync(Guid id, CancellationToken ct);

    Task<IEnumerable<T>> GetAllAsync(CancellationToken ct);

    Task<IEnumerable<T>> GetFilteredAsync(Expression<Func<T, bool>> predicate, CancellationToken ct);
}