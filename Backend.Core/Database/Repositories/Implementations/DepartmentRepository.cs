﻿using Backend.Core.Database.Repositories.Base;
using Backend.Core.Database.Repositories.Interfaces;
using Backend.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Core.Database.Repositories.Implementations;

public class DepartmentRepository : BaseRepository<DepartmentEntity>, IDepartmentRepository
{
    public DepartmentRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<DepartmentEntity>> GetByNameAsync(string name, CancellationToken ct)
    {
        return await _dbSet.AsNoTracking().Where(d => d.Name.Contains(name)).ToListAsync(ct);
    }

    public async Task<DepartmentEntity?> GetExactlyByNameAsync(string name, CancellationToken ct)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(d => d.Name == name, ct);
    }
}