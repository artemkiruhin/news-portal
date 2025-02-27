namespace Backend.Core.Models.DTOs.Response;

public record DepartmentResponse(
    Guid Id,
    string Name,
    int MemberCount,
    int PostCount,
    DateTime CreatedAt);