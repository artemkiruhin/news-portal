namespace Backend.Core.Models.DTOs;

public record UserStatisticsResponse(
    int TotalPosts,
    int TotalComments,
    int TotalReactions);