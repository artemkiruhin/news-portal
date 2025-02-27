using Backend.Core.Models.Entities;

namespace Backend.Core.Models.DTOs.Request;

public record LogFilterRequest(
    string? ContentContains,
    LogType? Type,
    DateTime? CreatedAfter,
    DateTime? CreatedBefore);