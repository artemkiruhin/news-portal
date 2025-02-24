using Backend.Core.Database.UnitOfWork;
using Backend.Core.Models.DTOs.Request;
using Backend.Core.Models.DTOs.Response;
using Backend.Core.Models.Entities;

namespace Backend.Core.UseCases;

public class ReactPostUseCase
{
    private readonly IUnitOfWork _database;

    public ReactPostUseCase(IUnitOfWork unitOfWork)
    {
        _database = unitOfWork;
    }
    
    public async Task<Result<Guid>> ExecuteAsync(ReactionCreateRequest request, CancellationToken ct)
    {
        try
        {
            var post = await _database.PostRepository.GetByIdAsync(request.PostId, ct);
            if (post == null) return Result<Guid>.Failure($"Новость с id: {request.PostId} не найдена!");
            
            var sender = await _database.UserRepository.GetByIdAsync(request.SenderId, ct);
            if (sender == null) return Result<Guid>.Failure($"Сотнрудник с id: {request.SenderId} не найден!");
            
            var reaction = ReactionEntity.Create(request.Type, request.PostId, request.SenderId);
            
            await _database.BeginTransactionAsync(ct);
            await _database.ReactionRepository.CreateAsync(reaction, ct);
            await _database.SaveChangesAsync(ct);
            await _database.CommitTransactionAsync(ct);
            return Result<Guid>.Success(post.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<Guid>.Failure("Ошибка создания " + e.Message);
        }
    }
}