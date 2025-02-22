using Backend.Core.Database.UnitOfWork;
using Backend.Core.Models.DTOs.Response;
using Backend.Core.Models.Entities;

namespace Backend.Core.UseCases;

public class ReactionStatsByPostUseCase
{
    private readonly IUnitOfWork _database;

    public ReactionStatsByPostUseCase(IUnitOfWork unitOfWork)
    {
        _database = unitOfWork;
    }
    
    public async Task<Result<ReactionStatsResponse>> ExecuteAsync(Guid postId)
    {
        try
        {
            var post = await _database.PostRepository.GetByIdAsync(postId);
            if (post == null) return Result<ReactionStatsResponse>.Failure($"Новость с id: {postId} не найдена!");
            
            var likes = post.Reactions.Count(x => x.Type == ReactionType.Like);
            var dislikes = post.Reactions.Count(x => x.Type == ReactionType.Dislike);
            var checks = post.Reactions.Count(x => x.Type == ReactionType.Checked);
            
            var result = new ReactionStatsResponse(likes, dislikes, checks);
            return Result<ReactionStatsResponse>.Success(result);
        }
        catch (Exception e)
        {
            return Result<ReactionStatsResponse>.Failure("Ошибка при обработке данных: " + e.Message);
        }
    }
}