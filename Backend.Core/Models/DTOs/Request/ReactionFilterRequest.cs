using Backend.Core.Models.Entities;

namespace Backend.Core.Models.DTOs.Request;

public record ReactionFilterRequest(
    ReactionType? Type,
    Guid? PostId,
    Guid? SenderId);