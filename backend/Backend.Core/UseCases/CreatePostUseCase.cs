﻿using Backend.Core.Database.UnitOfWork;
using Backend.Core.Models.DTOs.Request;
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
    
    public async Task<Result<Guid>> ExecuteAsync(PostCreateRequest request, CancellationToken ct)
    {
        try
        {
            var publisher = await _database.UserRepository.GetByIdAsync(request.PublisherId, ct);
            if (publisher == null) return Result<Guid>.Failure($"Пользователь с id: {request.PublisherId} не найден!");
            
            if (!publisher.HasPublishRights) return Result<Guid>.Failure("У пользователя нет доступа к публикации материала!");
            
            await _database.BeginTransactionAsync(ct);
            var post = PostEntity.Create(request.Title, request.Content, request.PublisherId, request.Subtitle);
            await _database.PostRepository.CreateAsync(post, ct);
            await _database.CommitTransactionAsync(ct);

            return Result<Guid>.Success(post.Id);
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            return Result<Guid>.Failure("Произошла ошибка при создании новости.");
        }
    }
}
