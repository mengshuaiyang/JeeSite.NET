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

// 定义class DictTypeConfiguration
// 定义类：DictTypeConfiguration
public class DictTypeConfiguration : IEntityTypeConfiguration<DictType>
{
    // 方法 Configure
    // 方法：Configure
    public void Configure(EntityTypeBuilder<DictType> builder)
    {
        builder.ToTable("Sys_Dict_Type");
        builder.HasKey(e => e.DictTypeCode);
        builder.Property(e => e.DictTypeCode).HasMaxLength(100);
        builder.Property(e => e.DictName).HasMaxLength(200);
        builder.Property(e => e.IsSys).HasMaxLength(1);
        builder.Property(e => e.Sort).HasColumnType("decimal(10,2)");
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(1);
    }
}
