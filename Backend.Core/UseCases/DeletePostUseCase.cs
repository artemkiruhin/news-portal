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
    
    public async Task<Result<Guid>> ExecuteAsync(Guid id)
    {
        try
        {
            var post = await _database.PostRepository.GetByIdAsync(id);
            if (post == null) return Result<Guid>.Failure($"Новость с id: {id} не найдена!");

            await _database.BeginTransactionAsync();
            post.IsDeleted = true;
            await _database.PostRepository.UpdateAsync(post);
            await _database.SaveChangesAsync();
            await _database.CommitTransactionAsync();
            return Result<Guid>.Success(post.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync();
            return Result<Guid>.Failure("Ошибка при удалении новости " + e.Message);
        }
    }
}