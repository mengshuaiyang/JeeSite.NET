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

// 定义class ConfigConfiguration
// 定义类：ConfigConfiguration
public class ConfigConfiguration : IEntityTypeConfiguration<Config>
{
    // 方法 Configure
    // 方法：Configure
    public void Configure(EntityTypeBuilder<Config> builder)
    {
        builder.ToTable("Sys_Config");
        builder.HasKey(e => e.ConfigKey);
        builder.Property(e => e.ConfigKey).HasMaxLength(100);
        builder.Property(e => e.ConfigName).HasMaxLength(200);
        builder.Property(e => e.ConfigValue).HasMaxLength(2000);
        builder.Property(e => e.IsSys).HasMaxLength(1);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.HasIndex(e => e.ConfigName);
    }
}
