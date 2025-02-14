namespace Backend.Core.Models.DTOs;

public record CommentResponse(
    Guid Id,
    string Content,
    AuthorShortResponse Author,
    DateTime CreatedAt,
    DateTime? EditedAt,
    CommentReplyResponse? Reply);