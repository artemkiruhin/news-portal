using Backend.Core.Database.UnitOfWork;
using Backend.Core.Models.DTOs.Response;
using Backend.Core.Models.Entities;

namespace Backend.Core.UseCases;

public class UpdatePostUseCase
{
    private readonly IUnitOfWork _database;

    public UpdatePostUseCase(IUnitOfWork unitOfWork)
    {
        _database = unitOfWork;
    }
    
    public async Task<Result<Guid>> ExecuteAsync(Guid id, string? title, string? subtitle, string? content, List<Guid>? departmentIds)
    {
        try
        {
            var post = await _database.PostRepository.GetByIdAsync(id);
            if (post == null) return Result<Guid>.Failure($"Новость с id: {id} не найден!");
            
            var departments = new List<DepartmentEntity>();
            if (departmentIds != null && departmentIds.Count != 0)
            {
                foreach (var depId in departmentIds)
                {
                    var department = await _database.DepartmentRepository.GetByIdAsync(depId);
                    if (department == null)
                        return Result<Guid>.Failure($"Отдела с id: {depId} не найден!");
                    
                    departments.Add(department);
                }
            }

            await _database.BeginTransactionAsync();

            if (!string.IsNullOrEmpty(title)) post.Title = title;
            post.Subtitle = subtitle;
            if (!string.IsNullOrEmpty(content)) post.Content = content;
            post.LastModifiedAt = DateTime.UtcNow;
            if (departmentIds != null) post.Departments = departments;
            await _database.PostRepository.UpdateAsync(post);
            await _database.SaveChangesAsync();
            await _database.CommitTransactionAsync();
            
            return Result<Guid>.Success(post.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync();
            return Result<Guid>.Failure("Ошибка изменения: " + e.Message);
        }
    }
}