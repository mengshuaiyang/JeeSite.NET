    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;
    // 引入 Microsoft.EntityFrameworkCore 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore
using Microsoft.EntityFrameworkCore;
    // 引入 Microsoft.EntityFrameworkCore.Metadata.Builders 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore.Metadata.Builders
using Microsoft.EntityFrameworkCore.Metadata.Builders;

// 定义 JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations
namespace JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations;

// 定义class FileEntityConfiguration
// 定义类：FileEntityConfiguration
public class FileEntityConfiguration : IEntityTypeConfiguration<FileEntity>
{
    // 方法 Configure
    // 方法：Configure
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
