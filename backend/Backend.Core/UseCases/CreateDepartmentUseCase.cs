using Backend.Core.Database.UnitOfWork;
using Backend.Core.Models.DTOs.Response;
using Backend.Core.Models.Entities;

namespace Backend.Core.UseCases;

public class CreateDepartmentUseCase
{
    private readonly IUnitOfWork _database;

    public CreateDepartmentUseCase(IUnitOfWork unitOfWork)
    {
        _database = unitOfWork;
    }
    
    public async Task<Result<Guid>> ExecuteAsync(string name, CancellationToken ct)
    {
        try
        {
            var department = await _database.DepartmentRepository.GetExactlyByNameAsync(name, ct);
            if (department != null) return Result<Guid>.Failure($"Отдел с названием: {name} уже существует!");
            
            var newDepartment = DepartmentEntity.Create(name);

            await _database.BeginTransactionAsync(ct);
            await _database.DepartmentRepository.CreateAsync(newDepartment, ct);
            await _database.SaveChangesAsync(ct);
            await _database.CommitTransactionAsync(ct);
            
            return Result<Guid>.Success(newDepartment.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<Guid>.Failure("Ошибка создания" + e.Message);
        }
    }
}