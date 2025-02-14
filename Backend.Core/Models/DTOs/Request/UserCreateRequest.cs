namespace Backend.Core.Models.DTOs.Request;

public record UserCreateRequest(
    string Username,
    string Password,
    string? Email,
    Guid DepartmentId);