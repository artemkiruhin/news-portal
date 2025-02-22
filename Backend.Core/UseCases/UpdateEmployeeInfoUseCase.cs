using Backend.Core.Database.UnitOfWork;
using Backend.Core.Models.DTOs.Response;
using Backend.Core.Services.Security.Hash;

namespace Backend.Core.UseCases;

public class UpdateEmployeeInfoUseCase
{
    private readonly IUnitOfWork _database;
    private readonly IHasher _hasher;

    public UpdateEmployeeInfoUseCase(IUnitOfWork unitOfWork, IHasher hasher)
    {
        _database = unitOfWork;
        _hasher = hasher;
    }
    
    public async Task<Result<Guid>> ExecuteAsync(Guid userId, CancellationToken ct, string? username, string? password, string? email, bool? hasPublishRights, Guid? departmentId)
    {
        try
        {
            if (username is null && password is null && email is null && hasPublishRights.HasValue && departmentId.HasValue)
                return Result<Guid>.Failure($"Нужно обязательно указать хотя бы 1 поле для изменения");
            var user = await _database.UserRepository.GetByIdAsync(userId, ct);
            if (user == null) return Result<Guid>.Failure($"Пользователь с id: {userId} не найден!");
            
            
            await _database.BeginTransactionAsync(ct);
            
            if (username is not null)
            {
                var userByUsername = await _database.UserRepository.GetByUsernameAsync(username, ct);
                if (userByUsername != null) return Result<Guid>.Failure($"Имя пользователя: {username} уже занято!");
                user.Username = username;
            }
            if (password is not null) _hasher.Hash(password);
            user.Email = email; 
            if (hasPublishRights.HasValue) user.HasPublishRights = hasPublishRights.Value;
            if (departmentId.HasValue)
            {
                var department = await _database.DepartmentRepository.GetByIdAsync(departmentId.Value, ct);
                if (department == null) return Result<Guid>.Failure($"Отдел с id: {departmentId} не существует!");
                user.DepartmentId = departmentId.Value;
            }
            
            await _database.UserRepository.UpdateAsync(user, ct);
            await _database.SaveChangesAsync(ct);
            await _database.CommitTransactionAsync(ct);
            
            return Result<Guid>.Success(user.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<Guid>.Failure("Ошибка при изменении: " + e.Message);
        }
    }
}