using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations;

public class FileEntityConfiguration : IEntityTypeConfiguration<FileEntity>
{
    public void Configure(EntityTypeBuilder<FileEntity> builder)
    {
        builder.ToTable("Sys_File_Entity");
        builder.HasKey(e => e.FileId);
        builder.Property(e => e.FileId).HasMaxLength(100);
        builder.Property(e => e.FileMd5).HasMaxLength(64).IsRequired();
        builder.Property(e => e.FilePath).HasMaxLength(1000).IsRequired();
        builder.Property(e => e.FileContentType).HasMaxLength(200).IsRequired();
        builder.Property(e => e.FileExtension).HasMaxLength(100).IsRequired();
        builder.Property(e => e.FileSize).HasColumnType("decimal(31,0)");
        builder.Property(e => e.FileMeta).HasMaxLength(500);
        builder.Property(e => e.FilePreview).HasMaxLength(1);
        builder.HasIndex(e => e.FileMd5);
    }
}
