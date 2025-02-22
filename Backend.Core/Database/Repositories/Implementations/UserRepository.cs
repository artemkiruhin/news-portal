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

    public async Task<IEnumerable<UserEntity>> GetPublishersAsync(CancellationToken ct, bool hasPublishedRights = true)
    {
        return await _dbSet.AsNoTracking().Where(e => e.HasPublishRights).ToListAsync(ct);
    }

    public async Task<UserEntity?> GetByUsernameAsync(string username, CancellationToken ct)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Username == username, ct);
    }

    public async Task<UserEntity?> GetByUsernameAndPasswordHashAsync(string username, string passwordHash, CancellationToken ct)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Username == username && e.PasswordHash == passwordHash, ct);
    }

    public async Task<IEnumerable<UserEntity>> GetByEmailAsync(string? email, CancellationToken ct)
    {
        return await _dbSet.AsNoTracking().Where(e => e.Email == email).ToListAsync(ct);
    }

    public async Task<IEnumerable<UserEntity>> GetDepartmentIdAsync(Guid departmentId, CancellationToken ct)
    {
        return await _dbSet.AsNoTracking().Where(e => e.DepartmentId == departmentId).ToListAsync(ct);
    }
}