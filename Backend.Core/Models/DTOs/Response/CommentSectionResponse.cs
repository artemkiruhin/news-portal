namespace Backend.Core.Models.DTOs.Response;

public record CommentSectionResponse(
    int TotalCount,
    IEnumerable<CommentResponse> TopComments);