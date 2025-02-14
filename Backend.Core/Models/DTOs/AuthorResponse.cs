namespace Backend.Core.Models.DTOs;

public record AuthorResponse(Guid Id, string Username, DepartmentShortResponse Department);