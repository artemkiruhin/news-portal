namespace Backend.Core.Models.DTOs;

public record UserResponse(
    Guid Id,
    string Username,
    string Email,
    bool HasPublishRights,
    DateTime CreatedAt,
    DepartmentShortResponse Department,
    UserStatisticsResponse Statistics);