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

// 定义class CompanyConfiguration
// 定义类：CompanyConfiguration
public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    // 方法 Configure
    // 方法：Configure
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Sys_Company");
        builder.HasKey(e => e.CompanyCode);
        builder.Property(e => e.CompanyCode).HasMaxLength(100);
        builder.Property(e => e.ViewCode).HasMaxLength(100);
        builder.Property(e => e.CompanyName).HasMaxLength(200);
        builder.Property(e => e.FullName).HasMaxLength(200);
        builder.Property(e => e.AreaCode).HasMaxLength(100);
        builder.Property(e => e.AreaName).HasMaxLength(200);
        builder.Property(e => e.ParentCode).HasMaxLength(100);
        builder.Property(e => e.ParentCodes).HasMaxLength(2000);
        builder.Property(e => e.TreeSort).HasMaxLength(2000);
        builder.Property(e => e.TreeSorts).HasMaxLength(2000);
        builder.Property(e => e.TreeNames).HasMaxLength(2000);
        builder.Property(e => e.TreeLeaf).HasMaxLength(1);
        builder.Property(e => e.CorpCode).HasMaxLength(100);
        builder.Property(e => e.CorpName).HasMaxLength(200);
        builder.Property(e => e.ExtendJson).HasColumnType("text");
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.HasIndex(e => e.ViewCode);
        builder.HasIndex(e => e.AreaCode);
    }
}
