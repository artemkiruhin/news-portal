namespace Backend.Core.Models.DTOs.Response;

public record UserStatisticsResponse(
    int TotalPosts,
    int TotalComments,
    int TotalReactions);