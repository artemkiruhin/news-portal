namespace Backend.Core.UseCases.Contracts;

public record RegisterSettings(string Username, string Password, string? Email, Guid DepartmentId, bool HasPublishedRights = false);