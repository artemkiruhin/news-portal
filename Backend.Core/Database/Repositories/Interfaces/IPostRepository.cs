using Backend.Core.Database.Repositories.Base;
using Backend.Core.Models.Entities;

namespace Backend.Core.Database.Repositories.Interfaces;

public interface IPostRepository : ICrudRepository<PostEntity>
{
    Task<IEnumerable<PostEntity>> GetByTextAsync(string? title, string? subtitle, string? content, CancellationToken ct);
    Task<IEnumerable<PostEntity>> GetByCreatedDateAsync(DateTime from, DateTime to, CancellationToken ct);
    Task<IEnumerable<PostEntity>> GetByLastModifiedDateAsync(DateTime from, DateTime to, CancellationToken ct);
    Task<IEnumerable<PostEntity>> GetByPublisherIdAsync(Guid publisherId, CancellationToken ct);
}