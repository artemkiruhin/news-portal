using Backend.Core.Database.UnitOfWork;
using Backend.Core.Models.DTOs.Response;
using Backend.Core.Services.Security.Hash;

namespace Backend.Core.UseCases;

public class ResetPasswordInfoUseCase
{
    private readonly IUnitOfWork _database;
    private readonly IHasher _hasher;

    public ResetPasswordInfoUseCase(IUnitOfWork unitOfWork, IHasher hasher)
    {
        _database = unitOfWork;
        _hasher = hasher;
    }
    
    public async Task<Result<string>> ExecuteAsync(Guid id, string newPassword, CancellationToken ct)
    {
        try
        {
            var hashedPassword = _hasher.Hash(newPassword);
            var user = await _database.UserRepository.GetByIdAsync(id, ct);
            if (user == null) return Result<string>.Failure($"Пользователь с id: {id} не найден!");

            await _database.BeginTransactionAsync(ct);
            user.PasswordHash = hashedPassword;
            await _database.UserRepository.UpdateAsync(user, ct);
            await _database.SaveChangesAsync(ct);
            await _database.CommitTransactionAsync(ct);
            return Result<string>.Success(hashedPassword);   
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<string>.Failure("Ошибка сброса пароля: " + e.Message);
        }
    }
}