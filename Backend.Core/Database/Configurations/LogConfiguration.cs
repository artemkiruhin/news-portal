using Backend.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Core.Database.Configurations;

public class LogConfiguration : IEntityTypeConfiguration<LogEntity>
{
    public void Configure(EntityTypeBuilder<LogEntity> builder)
    {
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id)
            .HasColumnType("uuid");
            //.HasDefaultValueSql("uuid_generate_v4()");
            
        builder.Property(l => l.Content)
            .HasColumnType("text")
            .IsRequired();
            
        builder.Property(l => l.Type)
            .HasConversion<string>();
            
        builder.Property(l => l.CreatedAt).IsRequired();
    }
}