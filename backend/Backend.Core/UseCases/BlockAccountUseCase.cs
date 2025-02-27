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
    
    public async Task<Result<Guid>> ExecuteAsync(Guid userId, CancellationToken ct)
    {
        try
        {
            var user = await _database.UserRepository.GetByIdAsync(userId, ct);
            if (user == null) return Result<Guid>.Failure($"Пользователя с id: {userId} не существует!");
            
            await _database.BeginTransactionAsync(ct);
            
            user.IsBlocked = true;
            await _database.UserRepository.UpdateAsync(user, ct);
            await _database.SaveChangesAsync(ct);
            
            await _database.CommitTransactionAsync(ct);
            return Result<Guid>.Success(user.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<Guid>.Failure("Ошибка блокировки" + e.Message);
        }
    }
}