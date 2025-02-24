namespace Backend.Core.Models.DTOs.Request;

public record PostCreateRequest(
    string Title,
    string? Subtitle,
    string Content,
    Guid PublisherId
);