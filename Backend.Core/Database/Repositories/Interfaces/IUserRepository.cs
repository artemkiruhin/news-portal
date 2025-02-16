using Backend.Core.Database.Repositories.Base;
using Backend.Core.Models.Entities;

namespace Backend.Core.Database.Repositories.Interfaces;

public interface IUserRepository : ICrudRepository<UserEntity>
{
    Task<IEnumerable<UserEntity>> GetPublishersAsync(bool hasPublishedRights = true);
    Task<UserEntity?> GetByUsernameAsync(string username);
    Task<UserEntity?> GetByUsernameAndPasswordHashAsync(string username, string passwordHash);
    Task<IEnumerable<UserEntity>> GetByEmailAsync(string? email);
    Task<IEnumerable<UserEntity>> GetDepartmentIdAsync(Guid departmentId);
}