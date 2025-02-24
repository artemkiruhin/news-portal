using Backend.Core.Database.UnitOfWork;
using Backend.Core.Models.DTOs.Request;
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
    
    public async Task<Result<Guid>> ExecuteAsync(UserUpdateRequest request, CancellationToken ct)
    {
        try
        {
            if (request.Username is null && request.NewPassword is null && request.Email is null && request is { HasPublishRights: not null, DepartmentId: not null })
                return Result<Guid>.Failure($"Нужно обязательно указать хотя бы 1 поле для изменения");
            var user = await _database.UserRepository.GetByIdAsync(request.UserId, ct);
            if (user == null) return Result<Guid>.Failure($"Пользователь с id: {request.UserId} не найден!");
            
            
            await _database.BeginTransactionAsync(ct);
            
            if (request.Username is not null)
            {
                var userByUsername = await _database.UserRepository.GetByUsernameAsync(request.Username, ct);
                if (userByUsername != null) return Result<Guid>.Failure($"Имя пользователя: {request.Username} уже занято!");
                user.Username = request.Username;
            }
            if (request.NewPassword is not null) _hasher.Hash(request.NewPassword);
            user.Email = request.Email; 
            if (request.HasPublishRights.HasValue) user.HasPublishRights = request.HasPublishRights.Value;
            if (request.DepartmentId.HasValue)
            {
                var department = await _database.DepartmentRepository.GetByIdAsync(request.DepartmentId.Value, ct);
                if (department == null) return Result<Guid>.Failure($"Отдел с id: {request.DepartmentId} не существует!");
                user.DepartmentId = request.DepartmentId.Value;
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