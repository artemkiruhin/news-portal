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
    
    public async Task<Result<Guid>> ExecuteAsync(string name)
    {
        try
        {
            var departments = await _database.DepartmentRepository.GetByNameAsync(name);
            var department = departments.FirstOrDefault();
            if (department != null) return Result<Guid>.Failure($"Отдел с названием: {name} уже существует!");
            
            var newDepartment = DepartmentEntity.Create(name);

            await _database.BeginTransactionAsync();
            await _database.DepartmentRepository.CreateAsync(newDepartment);
            await _database.SaveChangesAsync();
            await _database.CommitTransactionAsync();
            
            return Result<Guid>.Success(newDepartment.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync();
            return Result<Guid>.Failure("Ошибка создания" + e.Message);
        }
    }
}