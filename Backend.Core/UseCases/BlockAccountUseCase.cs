using Backend.Core.Database.UnitOfWork;
using Backend.Core.Models.DTOs.Response;

namespace Backend.Core.UseCases;

public class BlockAccountUseCase
{
    private readonly IUnitOfWork _database;

    public BlockAccountUseCase(IUnitOfWork unitOfWork)
    {
        _database = unitOfWork;
    }
    
    public async Task<Result<Guid>> ExecuteAsync(Guid userId)
    {
        try
        {
            var user = await _database.UserRepository.GetByIdAsync(userId);
            if (user == null) return Result<Guid>.Failure($"Пользователя с id: {userId} не существует!");
            
            await _database.BeginTransactionAsync();
            
            user.IsBlocked = true;
            await _database.UserRepository.UpdateAsync(user);
            await _database.SaveChangesAsync();
            
            await _database.CommitTransactionAsync();
            return Result<Guid>.Success(user.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync();
            return Result<Guid>.Failure("Ошибка блокировки" + e.Message);
        }
    }
}