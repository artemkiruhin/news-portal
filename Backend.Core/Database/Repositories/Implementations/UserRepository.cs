using Backend.Core.Database.Repositories.Base;
using Backend.Core.Database.Repositories.Interfaces;
using Backend.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Core.Database.Repositories.Implementations;

public class UserRepository : BaseRepository<UserEntity>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<UserEntity>> GetPublishersAsync(bool hasPublishedRights = true)
    {
        return await _dbSet.AsNoTracking().Where(e => e.HasPublishRights).ToListAsync();
    }

    public async Task<UserEntity?> GetByUsernameAsync(string username)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Username == username);
    }

    public async Task<UserEntity?> GetByUsernameAndPasswordHashAsync(string username, string passwordHash)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Username == username && e.PasswordHash == passwordHash);
    }

    public async Task<IEnumerable<UserEntity>> GetByEmailAsync(string? email)
    {
        return await _dbSet.AsNoTracking().Where(e => e.Email == email).ToListAsync();
    }

    public async Task<IEnumerable<UserEntity>> GetDepartmentIdAsync(Guid departmentId)
    {
        return await _dbSet.AsNoTracking().Where(e => e.DepartmentId == departmentId).ToListAsync();
    }
}