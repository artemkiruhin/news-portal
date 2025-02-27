using Backend.Core.Models.Entities;

namespace Backend.Core.Models.DTOs.Request;

public record ReactionUpdateRequest(ReactionType NewType);