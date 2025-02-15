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
        
        void ConfigureDateTime<TEntity>(string propertyName) where TEntity : class
        {
            modelBuilder.Entity<TEntity>()
                .Property<DateTime>(propertyName)
                .HasColumnType("timestamp without time zone")
                .IsRequired();
        }

        modelBuilder.Entity<UserEntity>(builder =>
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("uuid_generate_v4()");
            
            builder.Property(u => u.Username)
                .HasColumnType("varchar(20)")
                .IsRequired();
            builder.HasIndex(u => u.Username).IsUnique();
            
            builder.Property(u => u.PasswordHash)
                .HasColumnType("varchar(300)")
                .IsRequired();
            
            builder.Property(u => u.Email)
                .HasColumnType("varchar(50)")
                .IsRequired();
            builder.HasIndex(u => u.Email).IsUnique();
            
            builder.Property(u => u.HasPublishRights)
                .HasColumnType("boolean")
                .HasDefaultValue(false)
                .IsRequired();
            
            builder.Property(u => u.DepartmentId)
                .HasColumnType("uuid")
                .IsRequired();
            
            builder.HasOne(u => u.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(u => u.DepartmentId);
        });

        modelBuilder.Entity<PostEntity>(builder =>
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("uuid_generate_v4()");
            
            builder.Property(p => p.Title)
                .HasColumnType("varchar(50)")
                .IsRequired();
            
            builder.Property(p => p.Subtitle)
                .HasColumnType("varchar(100)");
            
            builder.Property(p => p.Content)
                .HasColumnType("text")
                .IsRequired();
            
            builder.Property(p => p.PublisherId)
                .HasColumnType("uuid")
                .IsRequired();
            
            builder.Property(p => p.PublishedAt)
                .HasColumnType("timestamp without time zone")
                .IsRequired();
            
            builder.Property(p => p.LastModifiedAt)
                .HasColumnType("timestamp without time zone");
            
            builder.HasOne(p => p.Publisher)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.PublisherId);
            
            builder.HasMany(p => p.Departments)
                .WithMany(d => d.Posts)
                .UsingEntity(j => j.ToTable("PostDepartments"));
        });

        modelBuilder.Entity<CommentEntity>(builder =>
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("uuid_generate_v4()");
            
            builder.Property(c => c.Content)
                .HasColumnType("text")
                .IsRequired();
            
            builder.Property(c => c.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .IsRequired();
            
            builder.Property(c => c.ReplyId)
                .HasColumnType("uuid");
            
            builder.HasOne(c => c.Sender)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.SenderId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ReactionEntity>(builder =>
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("uuid_generate_v4()");
            
            builder.Property(r => r.Type)
                .HasConversion<string>();
            
            builder.Property(r => r.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .IsRequired();
            
            builder.HasOne(r => r.Sender)
                .WithMany(u => u.Reactions)
                .HasForeignKey(r => r.SenderId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasOne(r => r.Post)
                .WithMany(p => p.Reactions)
                .HasForeignKey(r => r.PostId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DepartmentEntity>(builder =>
        {
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("uuid_generate_v4()");
            
            builder.Property(d => d.Name)
                .HasColumnType("varchar(50)")
                .IsRequired();
            builder.HasIndex(d => d.Name).IsUnique();
        });

        modelBuilder.Entity<LogEntity>(builder =>
        {
            builder.HasKey(l => l.Id);
            builder.Property(l => l.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("uuid_generate_v4()");
            
            builder.Property(l => l.Content)
                .HasColumnType("text")
                .IsRequired();
            
            builder.Property(l => l.Type)
                .HasConversion<string>();
            
            builder.Property(l => l.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .IsRequired();
        });
    }
}