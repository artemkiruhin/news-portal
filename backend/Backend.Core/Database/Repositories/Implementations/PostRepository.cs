using Backend.Core.Database.Repositories.Base;
using Backend.Core.Database.Repositories.Interfaces;
using Backend.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Core.Database.Repositories.Implementations;

public class PostRepository : BaseRepository<PostEntity>, IPostRepository
{
    public PostRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<PostEntity>> GetByTextAsync(string? title, string? subtitle, string? content, CancellationToken ct)
    {
        var query = _dbSet.AsNoTracking().AsQueryable();

        if (!string.IsNullOrEmpty(title))
            query = query.Where(p => p.Title.Contains(title));

        if (!string.IsNullOrEmpty(subtitle))
            query = query.Where(p => p.Subtitle != null && p.Subtitle.Contains(subtitle));

        if (!string.IsNullOrEmpty(content))
            query = query.Where(p => p.Content.Contains(content));

        return await query.ToListAsync(ct);
    }


    public async Task<IEnumerable<PostEntity>> GetByCreatedDateAsync(DateTime from, DateTime to, CancellationToken ct)
    {
        return await _dbSet.AsNoTracking().Where(p => p.PublishedAt >= from && p.PublishedAt <= to).ToListAsync(ct);
    }

    public async Task<IEnumerable<PostEntity>> GetByLastModifiedDateAsync(DateTime from, DateTime to, CancellationToken ct)
    {
        return await _dbSet.AsNoTracking().Where(p => p.LastModifiedAt >= from && p.LastModifiedAt <= to).ToListAsync(ct);
    }

    public async Task<IEnumerable<PostEntity>> GetByPublisherIdAsync(Guid publisherId, CancellationToken ct)
    {
        return await _dbSet.AsNoTracking().Where(p => p.PublisherId == publisherId).ToListAsync(ct);
    }
}