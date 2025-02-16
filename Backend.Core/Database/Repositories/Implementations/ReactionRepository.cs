using Backend.Core.Database.Repositories.Base;
using Backend.Core.Database.Repositories.Interfaces;
using Backend.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Core.Database.Repositories.Implementations;

public class ReactionRepository : BaseRepository<ReactionEntity>, IReactionRepository
{
    public ReactionRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ReactionEntity>> GetByTypeAsync(ReactionType type)
    {
        return await _dbSet.AsNoTracking().Where(e => e.Type == type).ToListAsync();
    }

    public async Task<IEnumerable<ReactionEntity>> GetByCreatedDateAsync(DateTime from, DateTime to)
    {
        return await _dbSet.AsNoTracking().Where(r => r.CreatedAt >= from && r.CreatedAt <= to).ToListAsync();
    }

    public async Task<IEnumerable<ReactionEntity>> GetByPostIdAsync(Guid postId)
    {
        return await _dbSet.AsNoTracking().Where(r => r.PostId == postId).ToListAsync();
    }

    public async Task<IEnumerable<ReactionEntity>> GetBySenderIdAsync(Guid senderId)
    {
        return await _dbSet.AsNoTracking().Where(r => r.SenderId == senderId).ToListAsync();
    }
}