﻿namespace Backend.Core.Models.DTOs.Response;

public record PostResponse(
    Guid Id,
    string Title,
    string? Subtitle,
    string ContentPreview,
    AuthorResponse Author,
    DateTime PublishedAt,
    DateTime LastModifiedAt,
    IEnumerable<DepartmentShortResponse> Departments,
    CommentSectionResponse Comments,
    ReactionStatsResponse Reactions);