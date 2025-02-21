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
    
    public async Task<Result<string>> ExecuteAsync(Guid id, string newPassword)
    {
        try
        {
            var hashedPassword = _hasher.Hash(newPassword);
            var user = await _database.UserRepository.GetByIdAsync(id);
            if (user == null) return Result<string>.Failure($"Пользователь с id: {id} не найден!");

            await _database.BeginTransactionAsync();
            user.PasswordHash = hashedPassword;
            await _database.UserRepository.UpdateAsync(user);
            await _database.SaveChangesAsync();
            await _database.CommitTransactionAsync();
            return Result<string>.Success(hashedPassword);   
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync();
            return Result<string>.Failure("Ошибка сброса пароля: " + e.Message);
        }
    }
}