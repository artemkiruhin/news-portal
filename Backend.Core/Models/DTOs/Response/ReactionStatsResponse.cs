using Backend.Core.Models.Entities;

namespace Backend.Core.Models.DTOs.Response;

public record ReactionStatsResponse(
    int TotalLikes,
    int TotalDislikes,
    int TotalChecked);