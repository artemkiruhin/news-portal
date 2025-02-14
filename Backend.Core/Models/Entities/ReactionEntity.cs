namespace Backend.Core.Models.Entities;

public class ReactionEntity
{
    public Guid Id { get; set; }
    public ReactionType Type { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid PostId { get; set; }
    public Guid SenderId { get; set; }
    
    public virtual PostEntity Post { get; set; } = null!;
    public virtual UserEntity Sender { get; set; } = null!;

    public static ReactionEntity Create (ReactionType type, Guid postId, Guid senderId)
    {
        return new()
        {
            Id = Guid.NewGuid(),
            Type = type,
            PostId = postId,
            SenderId = senderId,
            CreatedAt = DateTime.UtcNow
        };
    }
}