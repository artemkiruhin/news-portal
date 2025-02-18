using Backend.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Core.Database.Configurations;

public class ReactionConfiguration : IEntityTypeConfiguration<ReactionEntity>
{
    public void Configure(EntityTypeBuilder<ReactionEntity> builder)
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
        
        builder.Property(r => r.IsDeleted).HasColumnType("boolean").HasDefaultValue(false).IsRequired();
            
        builder.HasOne(r => r.Sender)
            .WithMany(u => u.Reactions)
            .HasForeignKey(r => r.SenderId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasOne(r => r.Post)
            .WithMany(p => p.Reactions)
            .HasForeignKey(r => r.PostId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}