namespace Backend.Core.Models.DTOs.Response;

public record AuthorResponse(Guid Id, string Username, DepartmentShortResponse Department);