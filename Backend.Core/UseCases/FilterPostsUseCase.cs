using Backend.Core.Database.UnitOfWork;
using Backend.Core.Models.DTOs.Request;
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
    
    public async Task<Result<List<PostResponse>>> ExecuteAsync(PostFilterRequest request, CancellationToken ct)
    {
        try
        {
            if (request.FullContent is null && !request.PublisherId.HasValue && request.PublishedAfter == null && request.PublishedBefore == null)
                return Result<List<PostResponse>>.Success([]);

            var posts = await _database.PostRepository.GetFilteredAsync(post =>
                    (!string.IsNullOrEmpty(request.FullContent) &&
                     (post.Title.Contains(request.FullContent) ||
                      (!string.IsNullOrEmpty(post.Subtitle) && post.Subtitle.Contains(request.FullContent)) ||
                      post.Content.Contains(request.FullContent))) ||
                    (request.PublisherId.HasValue && post.Publisher.Id == request.PublisherId.Value) ||
                    (request.PublishedAfter.HasValue && post.PublishedAt >= request.PublishedAfter.Value) ||
                    (request.PublishedBefore.HasValue && post.PublishedAt <= request.PublishedBefore.Value)
                , ct
            );
            
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