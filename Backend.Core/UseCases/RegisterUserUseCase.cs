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
    
    public async Task<Result<Guid>> ExecuteAsync(string username, string password, string? email, Guid departmentId, bool hasPublishedRights = false)
    {
        try
        {
            var user = await _database.UserRepository.GetByUsernameAsync(username);
            if (user != null) return Result<Guid>.Failure($"Пользователь с логином {username} уже зарегистрирован!");
            
            var department = await _database.DepartmentRepository.GetByIdAsync(departmentId);
            if (department == null) return Result<Guid>.Failure($"Отдела с id: {departmentId} не существует!");
            
            var hashedPassword = _hasher.Hash(password);

            var newUser = UserEntity.Create(username, hashedPassword, email, hasPublishedRights, departmentId);
            
            await _database.BeginTransactionAsync();
            
            var result = await _database.UserRepository.CreateAsync(newUser);
            await _database.SaveChangesAsync();
            
            await _database.CommitTransactionAsync();
            return Result<Guid>.Success(result.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync();
            return Result<Guid>.Failure("Ошибка регистрации" + e.Message);
        }
    }
}