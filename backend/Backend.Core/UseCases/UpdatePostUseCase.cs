using Backend.Core.Database.UnitOfWork;
using Backend.Core.Models.DTOs.Request;
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
    
    public async Task<Result<Guid>> ExecuteAsync(PostUpdateRequest request, CancellationToken ct)
    {
        try
        {
            var post = await _database.PostRepository.GetByIdAsync(request.Id, ct);
            if (post == null) return Result<Guid>.Failure($"Новость с id: {request.Id} не найден!");
            
            await _database.BeginTransactionAsync(ct);

            if (!string.IsNullOrEmpty(request.Title)) post.Title = request.Title;
            post.Subtitle = request.Subtitle;
            if (!string.IsNullOrEmpty(request.Content)) post.Content = request.Content;
            post.LastModifiedAt = DateTime.UtcNow;
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