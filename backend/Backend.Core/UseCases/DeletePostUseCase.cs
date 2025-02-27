using Backend.Core.Database.UnitOfWork;
using Backend.Core.Models.DTOs.Response;

namespace Backend.Core.UseCases;

public class DeletePostUseCase
{
    private readonly IUnitOfWork _database;

    public DeletePostUseCase(IUnitOfWork unitOfWork)
    {
        _database = unitOfWork;
    }
    
    public async Task<Result<Guid>> ExecuteAsync(Guid id, CancellationToken ct)
    {
        try
        {
            var post = await _database.PostRepository.GetByIdAsync(id, ct);
            if (post == null) return Result<Guid>.Failure($"Новость с id: {id} не найдена!");

            await _database.BeginTransactionAsync(ct);
            post.IsDeleted = true;
            await _database.PostRepository.UpdateAsync(post, ct);
            await _database.SaveChangesAsync(ct);
            await _database.CommitTransactionAsync(ct);
            return Result<Guid>.Success(post.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<Guid>.Failure("Ошибка при удалении новости " + e.Message);
        }
    }
}