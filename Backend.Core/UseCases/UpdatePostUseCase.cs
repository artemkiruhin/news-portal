using Backend.Core.Database.UnitOfWork;
using Backend.Core.Models.DTOs.Response;
using Backend.Core.Models.Entities;
using Backend.Core.UseCases.Contracts;

namespace Backend.Core.UseCases;

public class UpdatePostUseCase
{
    private readonly IUnitOfWork _database;

    public UpdatePostUseCase(IUnitOfWork unitOfWork)
    {
        _database = unitOfWork;
    }
    
    public async Task<Result<Guid>> ExecuteAsync(UpdatePostSettings settings, CancellationToken ct)
    {
        try
        {
            var post = await _database.PostRepository.GetByIdAsync(settings.Id, ct);
            if (post == null) return Result<Guid>.Failure($"Новость с id: {settings.Id} не найден!");
            
            var departments = new List<DepartmentEntity>();
            if (settings.DepartmentIds != null && settings.DepartmentIds.Count != 0)
            {
                foreach (var depId in settings.DepartmentIds)
                {
                    var department = await _database.DepartmentRepository.GetByIdAsync(depId, ct);
                    if (department == null)
                        return Result<Guid>.Failure($"Отдела с id: {depId} не найден!");
                    
                    departments.Add(department);
                }
            }

            await _database.BeginTransactionAsync(ct);

            if (!string.IsNullOrEmpty(settings.Title)) post.Title = settings.Title;
            post.Subtitle = settings.Subtitle;
            if (!string.IsNullOrEmpty(settings.Content)) post.Content = settings.Content;
            post.LastModifiedAt = DateTime.UtcNow;
            if (settings.DepartmentIds != null) post.Departments = departments;
            await _database.PostRepository.UpdateAsync(post, ct);
            await _database.SaveChangesAsync(ct);
            await _database.CommitTransactionAsync(ct);
            
            return Result<Guid>.Success(post.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<Guid>.Failure("Ошибка изменения: " + e.Message);
        }
    }
}