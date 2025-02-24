namespace Backend.Core.Models.DTOs.Request;

public record UserFilterRequest(
    string? Email,
    Guid? DepartmentId,
    bool? HasPublishRights);