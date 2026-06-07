using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations;

public class FileUploadConfiguration : IEntityTypeConfiguration<FileUpload>
{
    public void Configure(EntityTypeBuilder<FileUpload> builder)
    {
        builder.ToTable("Sys_File_Upload");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasMaxLength(100);
        builder.Property(e => e.FileId).HasMaxLength(100).IsRequired();
        builder.Property(e => e.FileName).HasMaxLength(500).IsRequired();
        builder.Property(e => e.FileType).HasMaxLength(20).IsRequired();
        builder.Property(e => e.FileSort).HasColumnType("decimal(10)");
        builder.Property(e => e.BizKey).HasMaxLength(100);
        builder.Property(e => e.BizType).HasMaxLength(100);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.HasIndex(e => e.FileId);
        builder.HasIndex(e => e.BizType);
        builder.HasIndex(e => e.BizKey);
        builder.HasIndex(e => e.CreateBy);
    }
}
