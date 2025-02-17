using Backend.Core.Database.Repositories.Base;
using Backend.Core.Models.Entities;

namespace Backend.Core.Database.Repositories.Interfaces;

public interface IDepartmentRepository : ICrudRepository<DepartmentEntity>
{
    Task<IEnumerable<DepartmentEntity>> GetByNameAsync(string name);
    Task<DepartmentEntity?> GetExactlyByNameAsync(string name);
}