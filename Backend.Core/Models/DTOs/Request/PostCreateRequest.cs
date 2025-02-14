namespace Backend.Core.Models.DTOs.Request;

public record PostCreateRequest(
    string Title,
    string? Subtitle,
    string Content,
    IEnumerable<Guid> DepartmentIds);