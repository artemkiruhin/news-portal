using Backend.Core.Database.Repositories.Base;
using Backend.Core.Models.Entities;

namespace Backend.Core.Database.Repositories.Interfaces;

public interface ILogRepository : ICrudRepository<LogEntity>
{
    Task<IEnumerable<LogEntity>> GetByTypeAsync(LogType type, CancellationToken ct);
    Task<IEnumerable<LogEntity>> GetByCreatedDateAsync(DateTime from, DateTime to, CancellationToken ct);
    Task<IEnumerable<LogEntity>> GetByMessageAsync(string message, CancellationToken ct);
}