using Backend.Core.Database.UnitOfWork;
using Backend.Core.Models.DTOs.Response;

namespace Backend.Core.UseCases;

public class UpdateDepartmentUseCase
{
    private readonly IUnitOfWork _database;

    public UpdateDepartmentUseCase(IUnitOfWork unitOfWork)
    {
        _database = unitOfWork;
    }
    
    public async Task<Result<Guid>> ExecuteAsync(Guid id, string name)
    {
        try
        {
            var department = await _database.DepartmentRepository.GetByIdAsync(id);
            if (department == null) return Result<Guid>.Failure($"Отдел с id: {id} не найден!");
            if (department.Name == name) return Result<Guid>.Failure($"Отдел уже имеет указанное имя!");

            await _database.BeginTransactionAsync();
            department.Name = name;
            await _database.DepartmentRepository.UpdateAsync(department);
            await _database.SaveChangesAsync(); 
            await _database.CommitTransactionAsync();
            return Result<Guid>.Success(department.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync();
            return Result<Guid>.Failure("Ошибка изменения" + e.Message);
        }
    }
}