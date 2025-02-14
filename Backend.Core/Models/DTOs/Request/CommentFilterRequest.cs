namespace Backend.Core.Models.DTOs.Request;

public record CommentFilterRequest(
    string? ContentContains,
    Guid? PostId,
    Guid? SenderId,
    DateTime? CreatedAfter);