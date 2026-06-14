    // 引入 JeeSiteNET.Infrastructure.EntityFrameworkCore 命名空间
// 引入命名空间：JeeSiteNET.Infrastructure.EntityFrameworkCore
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
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

// 定义class DictDataConfiguration
// 定义类：DictDataConfiguration
public class DictDataConfiguration : IEntityTypeConfiguration<DictData>
{
    // 方法 Configure
    // 方法：Configure
    public void Configure(EntityTypeBuilder<DictData> builder)
    {
        builder.ToTable("Sys_Dict_Data");
        builder.HasKey(e => e.DictCode);
        builder.Property(e => e.DictCode).HasMaxLength(100);
        builder.Property(e => e.ParentCode).HasMaxLength(100);
        builder.Property(e => e.ParentCodes).HasMaxLength(2000);
        builder.Property(e => e.TreeSort).HasMaxLength(2000);
        builder.Property(e => e.TreeSorts).HasMaxLength(2000);
        builder.Property(e => e.TreeNames).HasMaxLength(2000);
        builder.Property(e => e.TreeLeaf).HasMaxLength(1);
        builder.Property(e => e.DictType).HasMaxLength(100);
        builder.Property(e => e.DictLabel).HasMaxLength(200);
        builder.Property(e => e.DictValue).HasMaxLength(500);
        builder.Property(e => e.DictIcon).HasMaxLength(100);
        builder.Property(e => e.Description).HasMaxLength(500);
        builder.Property(e => e.CssStyle).HasMaxLength(200);
        builder.Property(e => e.CssClass).HasMaxLength(200);
        builder.Property(e => e.Sort).HasColumnType("decimal(10,2)");
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.ConfigureCorpFields();
        builder.ConfigureExtendFields();
        builder.HasIndex(e => e.DictType);
        builder.HasIndex(e => e.ParentCode);
    }
}
