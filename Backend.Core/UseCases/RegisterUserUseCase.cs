using Backend.Core.Database.UnitOfWork;
using Backend.Core.Models.DTOs.Response;
using Backend.Core.Models.Entities;
using Backend.Core.Services.Security.Hash;

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
    
    public async Task<Result<Guid>> ExecuteAsync(CancellationToken ct, string username, string password, string? email, Guid departmentId, bool hasPublishedRights = false)
    {
        try
        {
            var user = await _database.UserRepository.GetByUsernameAsync(username, ct);
            if (user != null) return Result<Guid>.Failure($"Пользователь с логином {username} уже зарегистрирован!");
            
            var department = await _database.DepartmentRepository.GetByIdAsync(departmentId, ct);
            if (department == null) return Result<Guid>.Failure($"Отдела с id: {departmentId} не существует!");
            
            var hashedPassword = _hasher.Hash(password);

            var newUser = UserEntity.Create(username, hashedPassword, email, hasPublishedRights, departmentId);
            
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