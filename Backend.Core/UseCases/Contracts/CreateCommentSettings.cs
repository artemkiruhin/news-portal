namespace Backend.Core.UseCases.Contracts;

public record CreateCommentSettings(string Content, Guid PostId, Guid SenderId, Guid? ReplyId = null);