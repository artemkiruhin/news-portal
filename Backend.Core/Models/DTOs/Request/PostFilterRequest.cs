namespace Backend.Core.Models.DTOs.Request;

public record PostFilterRequest(
    string? FullContent,
    Guid? PublisherId,
    DateTime? PublishedAfter,
    DateTime? PublishedBefore);