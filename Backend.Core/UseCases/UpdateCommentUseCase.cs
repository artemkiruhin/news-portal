using Backend.Core.Database.UnitOfWork;
using Backend.Core.Models.DTOs.Response;

namespace Backend.Core.UseCases;

public class UpdateCommentUseCase
{
    private readonly IUnitOfWork _database;

    public UpdateCommentUseCase(IUnitOfWork unitOfWork)
    {
        _database = unitOfWork;
    }
    
    public async Task<Result<Guid>> ExecuteAsync(Guid commentId, string content)
    {
        try
        {
            var comment = await _database.CommentRepository.GetByIdAsync(commentId);
            if (comment == null) return Result<Guid>.Failure($"Комментарий с id: {commentId} не найден!");
            
            await _database.BeginTransactionAsync();
            
            comment.Content = content;
            await _database.CommentRepository.UpdateAsync(comment);
            await _database.SaveChangesAsync();
            await _database.CommitTransactionAsync();
            
            return Result<Guid>.Success(comment.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync();
            return Result<Guid>.Failure("Ошибка изменения: " + e.Message);
        }
    }
}