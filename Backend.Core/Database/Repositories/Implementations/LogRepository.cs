using Backend.Core.Database.Repositories.Base;
using Backend.Core.Database.Repositories.Interfaces;
using Backend.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Core.Database.Repositories.Implementations;

public class LogRepository : BaseRepository<LogEntity>, ILogRepository
{
    public LogRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<LogEntity>> GetByTypeAsync(LogType type, CancellationToken ct)
    {
        return await _dbSet.AsNoTracking().Where(l => l.Type == type).ToListAsync(ct);
    }

    public async Task<IEnumerable<LogEntity>> GetByCreatedDateAsync(DateTime from, DateTime to, CancellationToken ct)
    {
        return await _dbSet.AsNoTracking().Where(l => l.CreatedAt >= from && l.CreatedAt <= to).ToListAsync(ct);
    }

    public async Task<IEnumerable<LogEntity>> GetByMessageAsync(string message, CancellationToken ct)
    {
        return await _dbSet.AsNoTracking().Where(l => l.Content.Contains(message)).ToListAsync(ct);
    }
}