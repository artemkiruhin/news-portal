using Backend.Core.Models.Entities;

namespace Backend.Core.Models.DTOs;

public record ReactionStatsResponse(
    int TotalLikes,
    int TotalDislikes,
    int TotalChecked,
    IEnumerable<ReactionType> UserReactions);