namespace Backend.Core.Models.Entities;

public class CommentEntity
{
    public Guid Id { get; set; }
    public required string Content { get; set; }
    public Guid PostId { get; set; }
    public Guid SenderId { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? ReplyId { get; set; }
    public virtual CommentEntity? Reply { get; set; }
    public virtual UserEntity Sender { get; set; } = null!;
    public virtual PostEntity Post { get; set; } = null!;

    public static CommentEntity Create(string content, Guid postId, Guid senderId, Guid? replyId = null)
    {
        return new()
        {
            Id = Guid.NewGuid(),
            Content = content,
            PostId = postId,
            SenderId = senderId,
            CreatedAt = DateTime.UtcNow,
            ReplyId = replyId
        };
    }
}