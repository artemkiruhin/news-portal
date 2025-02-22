namespace Backend.Core.Models.DTOs.Response;

public record PostResponse(
    Guid Id,
    string Title,
    string? Subtitle,
    string ContentPreview,
    string AuthorUsername,
    DateTime PublishedAt,
    DateTime LastModifiedAt,
    ReactionStatsResponse Reactions);