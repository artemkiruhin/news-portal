using Backend.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Core.Database;

public class CommentConfiguration : IEntityTypeConfiguration<CommentEntity>
{
    public void Configure(EntityTypeBuilder<CommentEntity> builder)
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
    }
}