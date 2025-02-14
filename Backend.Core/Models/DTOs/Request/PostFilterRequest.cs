namespace Backend.Core.Models.DTOs.Request;

public record PostFilterRequest(
    string? TitleContains,
    string? ContentContains,
    Guid? PublisherId,
    IEnumerable<Guid>? DepartmentIds,
    DateTime? PublishedAfter,
    DateTime? PublishedBefore);