namespace Backend.Core.Models.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public string? Email { get; set; }
    public bool HasPublishRights { get; set; }
    public Guid DepartmentId { get; set; }
    public bool IsBlocked { get; set; }
    public DateTime RegisteredAt { get; set; }
    public virtual ICollection<PostEntity> Posts { get; set; } = [];
    public virtual ICollection<CommentEntity> Comments { get; set; } = [];
    public virtual DepartmentEntity Department { get; set; } = null!;
    public virtual ICollection<ReactionEntity> Reactions { get; set; } = [];
    public static UserEntity Create(string username, string passwordHash, string? email, 
        bool hasPublishRights, Guid departmentId)
    {
        return new()
        {
            Id = Guid.NewGuid(),
            Username = username,
            PasswordHash = passwordHash,
            Email = email,
            HasPublishRights = hasPublishRights,
            DepartmentId = departmentId,
            IsBlocked = false,
            RegisteredAt = DateTime.UtcNow
        };
    }
}