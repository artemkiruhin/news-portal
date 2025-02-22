using Backend.Core.Database.UnitOfWork;
using Backend.Core.Models.DTOs.Response;
using Backend.Core.Services.Security.Hash;
using Backend.Core.UseCases.Contracts;

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
    
    public async Task<Result<Guid>> ExecuteAsync(UpdateEmployeeSettings settings, CancellationToken ct)
    {
        try
        {
            if (settings.Username is null && settings.Password is null && settings.Email is null && settings is { HasPublishRights: not null, DepartmentId: not null })
                return Result<Guid>.Failure($"Нужно обязательно указать хотя бы 1 поле для изменения");
            var user = await _database.UserRepository.GetByIdAsync(settings.UserId, ct);
            if (user == null) return Result<Guid>.Failure($"Пользователь с id: {settings.UserId} не найден!");
            
            
            await _database.BeginTransactionAsync(ct);
            
            if (settings.Username is not null)
            {
                var userByUsername = await _database.UserRepository.GetByUsernameAsync(settings.Username, ct);
                if (userByUsername != null) return Result<Guid>.Failure($"Имя пользователя: {settings.Username} уже занято!");
                user.Username = settings.Username;
            }
            if (settings.Password is not null) _hasher.Hash(settings.Password);
            user.Email = settings.Email; 
            if (settings.HasPublishRights.HasValue) user.HasPublishRights = settings.HasPublishRights.Value;
            if (settings.DepartmentId.HasValue)
            {
                var department = await _database.DepartmentRepository.GetByIdAsync(settings.DepartmentId.Value, ct);
                if (department == null) return Result<Guid>.Failure($"Отдел с id: {settings.DepartmentId} не существует!");
                user.DepartmentId = settings.DepartmentId.Value;
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