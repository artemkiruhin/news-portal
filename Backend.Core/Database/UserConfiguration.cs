using Backend.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Core.Database;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
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
    }
}