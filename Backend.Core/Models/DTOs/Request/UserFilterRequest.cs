namespace Backend.Core.Models.DTOs.Request;

public record UserFilterRequest(
    string? UsernameContains,
    string? EmailContains,
    Guid? DepartmentId,
    bool? HasPublishRights);