using Backend.Core.Database.Repositories.Base;
using Backend.Core.Models.Entities;

namespace Backend.Core.Database.Repositories.Interfaces;

public interface IUserRepository : ICrudRepository<UserEntity>
{
    Task<IEnumerable<UserEntity>> GetPublishersAsync(CancellationToken ct, bool hasPublishedRights = true);
    Task<UserEntity?> GetByUsernameAsync(string username, CancellationToken ct);
    Task<UserEntity?> GetByUsernameAndPasswordHashAsync(string username, string passwordHash, CancellationToken ct);
    Task<IEnumerable<UserEntity>> GetByEmailAsync(string? email, CancellationToken ct);
    Task<IEnumerable<UserEntity>> GetDepartmentIdAsync(Guid departmentId, CancellationToken ct);
}