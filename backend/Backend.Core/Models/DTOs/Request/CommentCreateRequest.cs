namespace Backend.Core.Models.DTOs.Request;

public record CommentCreateRequest(
    string Content,
    Guid? ReplyId);