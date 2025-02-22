using Backend.Core.Database.UnitOfWork;
using Backend.Core.Models.DTOs.Response;
using Backend.Core.Models.Entities;

namespace Backend.Core.UseCases;

public class ReplyCommentUseCase
{
    private readonly IUnitOfWork _database;
    
    
    public ReplyCommentUseCase(IUnitOfWork unitOfWork)
    {
        _database = unitOfWork;
    }

    public async Task<Result<Guid>> ExecuteAsync(string content, Guid postId, Guid senderId, Guid replyId, CancellationToken ct)
    {
        try
        {
            var post = await _database.PostRepository.GetByIdAsync(postId, ct);
            if (post == null) return Result<Guid>.Failure($"Новость с id: {postId} не найдена!");
            
            var sender = await _database.UserRepository.GetByIdAsync(senderId, ct);
            if (sender == null) return Result<Guid>.Failure($"Сотрудник с id: {senderId} не найден!");

           var reply = await _database.CommentRepository.GetByIdAsync(replyId, ct);
           if (reply == null) return Result<Guid>.Failure($"Комментарий с id: {replyId} не найден!");

            var comment = CommentEntity.Create(content, postId, senderId, replyId);
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