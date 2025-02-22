using Backend.Core.Database.UnitOfWork;
using Backend.Core.Models.DTOs.Response;
using Backend.Core.Models.Entities;

namespace Backend.Core.UseCases;

public class FilterPostsUseCase
{
    private readonly IUnitOfWork _database;

    public FilterPostsUseCase(IUnitOfWork unitOfWork)
    {
        _database = unitOfWork;
    }
    
    public async Task<Result<List<PostResponse>>> ExecuteAsync(string? fullContent, Guid? publisherId, List<Guid>? departmentIds, DateTime? startDate, DateTime? endDate, CancellationToken ct)
    {
        try
        {
            if (fullContent is null && !publisherId.HasValue && departmentIds == null && startDate == null && endDate == null)
                return Result<List<PostResponse>>.Success(new List<PostResponse>());

            var departments = new List<DepartmentEntity>();
            if (departmentIds != null && departmentIds.Count != 0)
            {
                foreach (var depId in departmentIds)
                {
                    var department = await _database.DepartmentRepository.GetByIdAsync(depId, ct);
                    if (department == null)
                        return Result<List<PostResponse>>.Failure($"Отдел с id: {depId} не найден!");
                    departments.Add(department);
                }
            }
            
            var posts = await _database.PostRepository.GetFilteredAsync(post =>
                (!string.IsNullOrEmpty(fullContent) && 
                    (post.Title.Contains(fullContent) || 
                    (!string.IsNullOrEmpty(post.Subtitle) && post.Subtitle.Contains(fullContent)) || 
                    post.Content.Contains(fullContent))) ||
                (publisherId.HasValue && post.Publisher.Id == publisherId.Value) ||
                (startDate.HasValue && post.PublishedAt >= startDate.Value) ||
                (endDate.HasValue && post.PublishedAt <= endDate.Value) ||
                (departments.Any() && post.Departments.Any(d => departments.Contains(d))), ct
            );

            if (posts == null || !posts.Any())
                return Result<List<PostResponse>>.Success(new List<PostResponse>());



            var postResponses = posts.Select(post => new PostResponse
            (
                post.Id,
                post.Title,
                post.Subtitle,
                post.Content,
                post.Publisher.Username,
                post.PublishedAt,
                post.LastModifiedAt,
                new ReactionStatsResponse(
                    post.Reactions.Count(x => x.Type == ReactionType.Like),
                    post.Reactions.Count(x => x.Type == ReactionType.Dislike),
                    post.Reactions.Count(x => x.Type == ReactionType.Checked)
                )
            )).ToList();

            return Result<List<PostResponse>>.Success(postResponses);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result<List<PostResponse>>.Failure($"Произошла ошибка при фильтрации постов: {e.Message}");
        }
    }
}