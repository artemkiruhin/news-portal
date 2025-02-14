namespace Backend.Core.Models.DTOs;

public record CommentSectionResponse(
    int TotalCount,
    IEnumerable<CommentResponse> TopComments);