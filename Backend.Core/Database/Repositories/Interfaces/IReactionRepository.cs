using Backend.Core.Database.Repositories.Base;
using Backend.Core.Models.Entities;

namespace Backend.Core.Database.Repositories.Interfaces;

public interface IReactionRepository : ICrudRepository<ReactionEntity>
{
    Task<IEnumerable<ReactionEntity>> GetByTypeAsync(ReactionType type, CancellationToken ct);
    Task<IEnumerable<ReactionEntity>> GetByCreatedDateAsync(DateTime from, DateTime to, CancellationToken ct);
    Task<IEnumerable<ReactionEntity>> GetByPostIdAsync(Guid postId, CancellationToken ct);
    Task<IEnumerable<ReactionEntity>> GetBySenderIdAsync(Guid senderId, CancellationToken ct);
}