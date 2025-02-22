using Backend.Core.Database.UnitOfWork;
using Backend.Core.Models.DTOs.Response;
using Backend.Core.Models.Entities;
using Backend.Core.UseCases.Contracts;

namespace Backend.Core.UseCases;

public class CommentPostUseCase
{
    private readonly IUnitOfWork _database;

    public CommentPostUseCase(IUnitOfWork unitOfWork)
    {
        _database = unitOfWork;
    }
    
    public async Task<Result<Guid>> ExecuteAsync(CreateCommentSettings settings, CancellationToken ct)
    {
        try
        {
            var post = await _database.PostRepository.GetByIdAsync(settings.PostId, ct);
            if (post == null) return Result<Guid>.Failure($"Новость с id: {settings.PostId} не найдена!");
            
            var sender = await _database.UserRepository.GetByIdAsync(settings.SenderId, ct);
            if (sender == null) return Result<Guid>.Failure($"Сотрудник с id: {settings.SenderId} не найден!");

            if (settings.ReplyId.HasValue)
            {
                var reply = await _database.CommentRepository.GetByIdAsync(settings.ReplyId.Value, ct);
                if (reply == null) return Result<Guid>.Failure($"Комментарий с id: {settings.ReplyId.Value} не найден!");
            }

            var comment = CommentEntity.Create(settings.Content, settings.PostId, settings.SenderId, settings.ReplyId);
            await _database.BeginTransactionAsync(ct);            
            await _database.CommentRepository.CreateAsync(comment, ct);
            await _database.CommitTransactionAsync(ct);
            
            return Result<Guid>.Success(post.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<Guid>.Failure("Ошибка создания: " + e.Message);
        }
    }
}