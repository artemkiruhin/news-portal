using Backend.Core.Database.UnitOfWork;
using Backend.Core.Models.DTOs.Response;
using Backend.Core.Models.Entities;
using Backend.Core.UseCases.Contracts;

namespace Backend.Core.UseCases;

public class CreatePostUseCase
{
    private readonly IUnitOfWork _database;

    public CreatePostUseCase(IUnitOfWork unitOfWork)
    {
        _database = unitOfWork;
    }
    
    public async Task<Result<Guid>> ExecuteAsync(CreatePostSettings settings)
    {
        try
        {
            var publisher = await _database.UserRepository.GetByIdAsync(settings.PublisherId);
            if (publisher == null) return Result<Guid>.Failure($"Пользователь с id: {settings.PublisherId} не найден!");
            
            if (!publisher.HasPublishRights) return Result<Guid>.Failure("У пользователя нет доступа к публикации материала!");
            
            var departments = new List<DepartmentEntity>();
            if (settings.DepartmentIds.Count != 0)
            {
                foreach (var depId in settings.DepartmentIds)
                {
                    var department = await _database.DepartmentRepository.GetByIdAsync(depId);
                    if (department == null)
                        return Result<Guid>.Failure($"Отдела с id: {depId} не найден!");
                    
                    departments.Add(department);
                }
            }
            await _database.BeginTransactionAsync();
            var post = PostEntity.Create(settings.Title, settings.Content, settings.PublisherId, departments, settings.SubTitle);
            await _database.PostRepository.CreateAsync(post);
            await _database.CommitTransactionAsync();

            return Result<Guid>.Success(post.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync();
            return Result<Guid>.Failure("Произошла ошибка при создании новости.");
        }
    }
}
