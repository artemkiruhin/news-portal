using Backend.Core.Database.Repositories.Base;
using Backend.Core.Models.Entities;

namespace Backend.Core.Database.Repositories.Interfaces;

public interface ICommentRepository : ICrudRepository<CommentEntity>
{
    Task<IEnumerable<CommentEntity>> GetByContentAsync(string content, CancellationToken ct);
    Task<IEnumerable<CommentEntity>> GetByPostIdAsync(Guid postId, CancellationToken ct);
    Task<IEnumerable<CommentEntity>> GetBySenderIdAsync(Guid senderId, CancellationToken ct);
    Task<IEnumerable<CommentEntity>> GetByCreatedDateAsync(DateTime from, DateTime to, CancellationToken ct);
    Task<IEnumerable<CommentEntity>> GetByReplyIdAsync(Guid replyId, CancellationToken ct);
}