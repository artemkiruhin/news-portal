namespace Backend.Core.Models.DTOs.Response;

public record CommentReplyResponse(
    Guid Id,
    string ContentPreview,
    AuthorShortResponse Author);