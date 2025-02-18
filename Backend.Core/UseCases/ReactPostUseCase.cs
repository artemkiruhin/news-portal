using Backend.Core.Database.UnitOfWork;
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
    
    public async Task<Result<Guid>> ExecuteAsync(Guid postId, Guid senderId, ReactionType type)
    {
        try
        {
            var post = await _database.PostRepository.GetByIdAsync(postId);
            if (post == null) return Result<Guid>.Failure($"Новость с id: {postId} не найдена!");
            
            var sender = await _database.UserRepository.GetByIdAsync(senderId);
            if (sender == null) return Result<Guid>.Failure($"Сотнрудник с id: {senderId} не найден!");
            
            var reaction = ReactionEntity.Create(type, postId, senderId);
            
            await _database.BeginTransactionAsync();
            await _database.ReactionRepository.CreateAsync(reaction);
            await _database.SaveChangesAsync();
            await _database.CommitTransactionAsync();
            return Result<Guid>.Success(post.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync();
            return Result<Guid>.Failure("Ошибка создания " + e.Message);
        }
    }
}