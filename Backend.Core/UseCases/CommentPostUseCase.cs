using Backend.Core.Database.UnitOfWork;
using Backend.Core.Models.DTOs.Response;
using Backend.Core.Models.Entities;

namespace Backend.Core.UseCases;

public class CommentPostUseCase
{
    private readonly IUnitOfWork _database;

    public CommentPostUseCase(IUnitOfWork unitOfWork)
    {
        _database = unitOfWork;
    }
    
    public async Task<Result<Guid>> ExecuteAsync(string content, Guid postId, Guid senderId, Guid? replyId = null)
    {
        try
        {
            var post = await _database.PostRepository.GetByIdAsync(postId);
            if (post == null) return Result<Guid>.Failure($"Новость с id: {postId} не найдена!");
            
            var sender = await _database.UserRepository.GetByIdAsync(senderId);
            if (sender == null) return Result<Guid>.Failure($"Сотрудник с id: {senderId} не найден!");

            if (replyId.HasValue)
            {
                var reply = await _database.CommentRepository.GetByIdAsync(replyId.Value);
                if (reply == null) return Result<Guid>.Failure($"Комментарий с id: {replyId.Value} не найден!");
            }

            var comment = CommentEntity.Create(content, postId, senderId, replyId);
            await _database.BeginTransactionAsync();            
            await _database.CommentRepository.CreateAsync(comment);
            await _database.CommitTransactionAsync();
            
            return Result<Guid>.Success(post.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync();
            return Result<Guid>.Failure("Ошибка создания: " + e.Message);
        }
    }
}