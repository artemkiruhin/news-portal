using Backend.Core.Database.Repositories.Base;
using Backend.Core.Models.Entities;

namespace Backend.Core.Database.Repositories.Interfaces;

public interface IReactionRepository : ICrudRepository<ReactionEntity>
{
    Task<IEnumerable<ReactionEntity>> GetByTypeAsync(ReactionType type);
    Task<IEnumerable<ReactionEntity>> GetByCreatedDateAsync(DateTime from, DateTime to);
    Task<IEnumerable<ReactionEntity>> GetByPostIdAsync(Guid postId);
    Task<IEnumerable<ReactionEntity>> GetBySenderIdAsync(Guid senderId);
}