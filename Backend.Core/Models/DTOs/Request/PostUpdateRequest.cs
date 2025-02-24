namespace Backend.Core.Models.DTOs.Request;

public record PostUpdateRequest(
    Guid Id,
    string? Title,
    string? Subtitle,
    string? Content
);