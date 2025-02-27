namespace Backend.Core.UseCases.Contracts;

public record CreatePostSettings(string Title, string? SubTitle, string Content, Guid PublisherId, List<Guid> DepartmentIds);