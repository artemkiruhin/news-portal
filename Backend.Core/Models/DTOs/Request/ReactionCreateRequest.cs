using Backend.Core.Models.Entities;

namespace Backend.Core.Models.DTOs.Request;

public record ReactionCreateRequest(Guid PostId, Guid SenderId, ReactionType Type);