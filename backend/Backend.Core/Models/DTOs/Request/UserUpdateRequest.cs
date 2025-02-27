namespace Backend.Core.Models.DTOs.Request;

public record UserUpdateRequest(
    Guid UserId,
    string? Username,
    string? NewPassword,
    string? Email,
    bool? HasPublishRights,
    Guid? DepartmentId);