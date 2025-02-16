using Backend.Core.Database.Repositories.Base;
using Backend.Core.Models.Entities;

namespace Backend.Core.Database.Repositories.Interfaces;

public interface IPostRepository : ICrudRepository<PostEntity>
{
    Task<IEnumerable<PostEntity>> GetByTextAsync(string? title, string? subtitle, string? content);
    Task<IEnumerable<PostEntity>> GetByCreatedDateAsync(DateTime from, DateTime to);
    Task<IEnumerable<PostEntity>> GetByLastModifiedDateAsync(DateTime from, DateTime to);
    Task<IEnumerable<PostEntity>> GetByPublisherIdAsync(Guid publisherId);
}