namespace Backend.Core.Models.DTOs.Request;

public record PostUpdateRequest(
    string? Title,
    string? Subtitle,
    string? Content,
    IEnumerable<Guid>? DepartmentIds);