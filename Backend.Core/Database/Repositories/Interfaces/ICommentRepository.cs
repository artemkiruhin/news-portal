using Backend.Core.Database.Repositories.Base;
using Backend.Core.Models.Entities;

namespace Backend.Core.Database.Repositories.Interfaces;

public interface ICommentRepository : ICrudRepository<CommentEntity>
{
    Task<IEnumerable<CommentEntity>> GetByContentAsync(string content);
    Task<IEnumerable<CommentEntity>> GetByPostIdAsync(Guid postId);
    Task<IEnumerable<CommentEntity>> GetBySenderIdAsync(Guid senderId);
    Task<IEnumerable<CommentEntity>> GetByCreatedDateAsync(DateTime from, DateTime to);
    Task<IEnumerable<CommentEntity>> GetByReplyIdAsync(Guid replyId);
}