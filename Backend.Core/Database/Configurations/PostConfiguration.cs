using Backend.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Core.Database.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<PostEntity>
{
    public void Configure(EntityTypeBuilder<PostEntity> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasColumnType("uuid");
            //.HasDefaultValueSql("uuid_generate_v4()");
            
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
            
        builder.Property(p => p.PublishedAt).IsRequired();
            
        //builder.Property(p => p.LastModifiedAt)
        //    .HasColumnType("datetime");
        
        builder.Property(p => p.IsDeleted).HasColumnType("boolean").IsRequired();
            
        builder.HasOne(p => p.Publisher)
            .WithMany(u => u.Posts)
            .HasForeignKey(p => p.PublisherId);
            
        builder.HasMany(p => p.Departments)
            .WithMany(d => d.Posts)
            .UsingEntity(j => j.ToTable("PostDepartments"));
    }
}