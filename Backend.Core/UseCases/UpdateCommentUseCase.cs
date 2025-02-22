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
    
    public async Task<Result<Guid>> ExecuteAsync(Guid commentId, string content, CancellationToken ct)
    {
        try
        {
            var comment = await _database.CommentRepository.GetByIdAsync(commentId, ct);
            if (comment == null) return Result<Guid>.Failure($"Комментарий с id: {commentId} не найден!");
            
            await _database.BeginTransactionAsync(ct);
            
            comment.Content = content;
            await _database.CommentRepository.UpdateAsync(comment, ct);
            await _database.SaveChangesAsync(ct);
            await _database.CommitTransactionAsync(ct);
            
            return Result<Guid>.Success(comment.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<Guid>.Failure("Ошибка изменения: " + e.Message);
        }
    }
}