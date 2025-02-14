namespace Backend.Core.Models.DTOs.Request;

public record DepartmentFilterRequest(
    string? NameContains,
    bool? HasPosts);