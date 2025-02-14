namespace Backend.Core.Models.DTOs;

public record CommentReplyResponse(
    Guid Id,
    string ContentPreview,
    AuthorShortResponse Author);