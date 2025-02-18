namespace Backend.Core.Models.Entities;

public class PostEntity
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public string? Subtitle { get; set; }
    public required string Content { get; set; }
    public bool IsDeleted { get; set; }
    public Guid PublisherId { get; set; }
    public DateTime PublishedAt { get; set; }
    public DateTime LastModifiedAt { get; set; }
    public virtual ICollection<CommentEntity> Comments { get; set; } = [];
    public virtual ICollection<DepartmentEntity> Departments { get; set; } = [];
    public virtual UserEntity Publisher { get; set; } = null!;
    public virtual ICollection<ReactionEntity> Reactions { get; set; } = [];

    public static PostEntity Create(string title, string content, Guid publisherId, 
        ICollection<DepartmentEntity> departments, string? subtitle = null)
    {
        var now = DateTime.UtcNow;
        return new()
        {
            Id = Guid.NewGuid(),
            Title = title,
            Subtitle = subtitle,
            Content = content,
            PublisherId = publisherId,
            PublishedAt = now,
            LastModifiedAt = now,
            Departments = departments,
            IsDeleted = false,
        };
    }
}