using Backend.Core.Database.UnitOfWork;
using Backend.Core.Models.DTOs.Response;
using Backend.Core.Models.Entities;
using Backend.Core.Services.Security.Hash;
using Backend.Core.UseCases.Contracts;

namespace Backend.Core.UseCases;

public class RegisterUserUseCase
{
    private readonly IUnitOfWork _database;
    private readonly IHasher _hasher;

    public RegisterUserUseCase(IUnitOfWork database, IHasher hasher)
    {
        _database = database;
        _hasher = hasher;
    }
    
    public async Task<Result<Guid>> ExecuteAsync(RegisterSettings settings, CancellationToken ct)
    {
        try
        {
            var user = await _database.UserRepository.GetByUsernameAsync(settings.Username, ct);
            if (user != null) return Result<Guid>.Failure($"Пользователь с логином {settings.Username} уже зарегистрирован!");
            
            var department = await _database.DepartmentRepository.GetByIdAsync(settings.DepartmentId, ct);
            if (department == null) return Result<Guid>.Failure($"Отдела с id: {settings.DepartmentId} не существует!");
            
            var hashedPassword = _hasher.Hash(settings.Password);

            var newUser = UserEntity.Create(settings.Username, hashedPassword, settings.Email, settings.HasPublishedRights, settings.DepartmentId);
            
            await _database.BeginTransactionAsync(ct);
            
            var result = await _database.UserRepository.CreateAsync(newUser, ct);
            await _database.SaveChangesAsync(ct);
            
            await _database.CommitTransactionAsync(ct);
            return Result<Guid>.Success(result.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<Guid>.Failure("Ошибка регистрации" + e.Message);
        }
    }
}