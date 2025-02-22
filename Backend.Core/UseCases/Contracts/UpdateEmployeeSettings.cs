namespace Backend.Core.UseCases.Contracts;

public record UpdateEmployeeSettings(
    Guid UserId,
    string? Username,
    string? Password,
    string? Email,
    bool? HasPublishRights,
    Guid? DepartmentId);