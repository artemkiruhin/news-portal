namespace Backend.Core.Models.Entities;

public class LogEntity
{
    public Guid Id { get; set; }
    public required string Content { get; set; }
    public LogType Type { get; set; }
    public DateTime CreatedAt { get; set; }

    public static LogEntity Create(string content, LogType type = LogType.Info)
    {
        return new()
        {
            Id = Guid.NewGuid(),
            Content = content,
            Type = type,
            CreatedAt = DateTime.UtcNow
        };
    }
}