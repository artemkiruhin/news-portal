using Backend.Core.Models.Entities;

namespace Backend.Core.Models.DTOs;

public record LogResponse(
    Guid Id,
    string ContentPreview,
    LogType Type,
    DateTime CreatedAt);