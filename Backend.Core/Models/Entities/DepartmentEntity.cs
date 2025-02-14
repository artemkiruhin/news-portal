namespace Backend.Core.Models.Entities;

public class DepartmentEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public virtual ICollection<UserEntity> Employees { get; set; } = [];
    public virtual ICollection<PostEntity> Posts { get; set; } = [];

    public static DepartmentEntity Create(string name)
    {
        return new()
        {
            Id = Guid.NewGuid(),
            Name = name
        };
    }
}