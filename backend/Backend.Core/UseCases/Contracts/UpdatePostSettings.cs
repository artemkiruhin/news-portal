namespace Backend.Core.UseCases.Contracts;

public record UpdatePostSettings(Guid Id, string? Title, string? Subtitle, string? Content, List<Guid>? DepartmentIds);