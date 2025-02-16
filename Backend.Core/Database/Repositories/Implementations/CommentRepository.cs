﻿using Backend.Core.Database.Repositories.Base;
using Backend.Core.Database.Repositories.Interfaces;
using Backend.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Core.Database.Repositories.Implementations;

public class CommentRepository : BaseRepository<CommentEntity>, ICommentRepository
{
    public CommentRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<CommentEntity>> GetByContentAsync(string content)
    {
        return await _dbSet.AsNoTracking().Where(c => c.Content.Contains(content)).ToListAsync();
    }

    public async Task<IEnumerable<CommentEntity>> GetByPostIdAsync(Guid postId)
    {
        return await _dbSet.AsNoTracking().Where(c => c.PostId == postId).ToListAsync();
    }

    public async Task<IEnumerable<CommentEntity>> GetBySenderIdAsync(Guid senderId)
    {
        return await _dbSet.AsNoTracking().Where(c => c.SenderId == senderId).ToListAsync();
    }

    public async Task<IEnumerable<CommentEntity>> GetByCreatedDateAsync(DateTime from, DateTime to)
    {
        return await _dbSet.AsNoTracking().Where(c => c.CreatedAt >= from && c.CreatedAt <= to).ToListAsync();
    }

    public async Task<IEnumerable<CommentEntity>> GetByReplyIdAsync(Guid replyId)
    {
        return await _dbSet.AsNoTracking().Where(c => c.ReplyId == replyId).ToListAsync();
    }
}