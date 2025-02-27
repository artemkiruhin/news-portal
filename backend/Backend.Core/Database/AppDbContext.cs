using Backend.Core.Database.Configurations;
using Backend.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Core.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<CommentEntity> Comments { get; set; }
    public DbSet<DepartmentEntity> Departments { get; set; }
    public DbSet<LogEntity> Logs { get; set; }
    public DbSet<PostEntity> Posts { get; set; }
    public DbSet<ReactionEntity> Reactions { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasPostgresExtension("pgcrypto");
        
        modelBuilder.ApplyConfiguration(new CommentConfiguration());
        modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
        modelBuilder.ApplyConfiguration(new LogConfiguration());
        modelBuilder.ApplyConfiguration(new PostConfiguration());
        modelBuilder.ApplyConfiguration(new ReactionConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}