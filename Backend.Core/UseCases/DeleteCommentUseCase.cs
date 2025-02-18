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
    
    public async Task<Result<Guid>> ExecuteAsync(Guid id)
    {
        try
        {
            var comment = await _database.CommentRepository.GetByIdAsync(id);
            if (comment == null) return Result<Guid>.Failure($"Комментарий с id: {id} не найден!");

            await _database.BeginTransactionAsync();
            comment.IsDeleted = true;
            await _database.CommentRepository.UpdateAsync(comment);
            await _database.SaveChangesAsync();
            await _database.CommitTransactionAsync();
            return Result<Guid>.Success(comment.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync();
            return Result<Guid>.Failure("Ошибка при удалении комментария " + e.Message);
        }
    }
}