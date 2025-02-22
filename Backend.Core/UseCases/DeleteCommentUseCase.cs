using Backend.Core.Database.UnitOfWork;
using Backend.Core.Models.DTOs.Response;

namespace Backend.Core.UseCases;

public class DeleteCommentUseCase
{
    private readonly IUnitOfWork _database;

    public DeleteCommentUseCase(IUnitOfWork unitOfWork)
    {
        _database = unitOfWork;
    }
    
    public async Task<Result<Guid>> ExecuteAsync(Guid id, CancellationToken ct)
    {
        try
        {
            var comment = await _database.CommentRepository.GetByIdAsync(id, ct);
            if (comment == null) return Result<Guid>.Failure($"Комментарий с id: {id} не найден!");

            await _database.BeginTransactionAsync(ct);
            comment.IsDeleted = true;
            await _database.CommentRepository.UpdateAsync(comment, ct);
            await _database.SaveChangesAsync(ct);
            await _database.CommitTransactionAsync(ct);
            return Result<Guid>.Success(comment.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<Guid>.Failure("Ошибка при удалении комментария " + e.Message);
        }
    }
}