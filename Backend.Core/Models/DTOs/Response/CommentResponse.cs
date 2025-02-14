namespace Backend.Core.Models.DTOs.Response;

public record CommentResponse(
    Guid Id,
    string Content,
    AuthorShortResponse Author,
    DateTime CreatedAt,
    DateTime? EditedAt,
    CommentReplyResponse? Reply);