using Backend.Core.Database.Repositories.Base;
using Backend.Core.Models.Entities;

namespace Backend.Core.Database.Repositories.Interfaces;

public interface ILogRepository : ICrudRepository<LogEntity>
{
    Task<IEnumerable<LogEntity>> GetByTypeAsync(LogType type);
    Task<IEnumerable<LogEntity>> GetByCreatedDateAsync(DateTime from, DateTime to);
    Task<IEnumerable<PostEntity>> GetByMessageAsync(string message);
}