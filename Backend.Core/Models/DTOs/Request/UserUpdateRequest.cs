namespace Backend.Core.Models.DTOs.Request;

public record UserUpdateRequest(
    string? Username,
    string? NewPassword,
    string? Email,
    bool? HasPublishRights,
    Guid? DepartmentId);