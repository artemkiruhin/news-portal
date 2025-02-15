using Backend.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Core.Database.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<DepartmentEntity>
{
    public void Configure(EntityTypeBuilder<DepartmentEntity> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id)
            .HasColumnType("uuid")
            .HasDefaultValueSql("uuid_generate_v4()");
            
        builder.Property(d => d.Name)
            .HasColumnType("varchar(50)")
            .IsRequired();
        builder.HasIndex(d => d.Name).IsUnique();
    }
}