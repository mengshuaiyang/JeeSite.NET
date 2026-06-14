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

// 定义class LangConfiguration
// 定义类：LangConfiguration
public class LangConfiguration : IEntityTypeConfiguration<Lang>
{
    // 方法 Configure
    // 方法：Configure
    public void Configure(EntityTypeBuilder<Lang> builder)
    {
        builder.ToTable("Sys_Lang");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.ModuleCode).HasMaxLength(100);
        builder.Property(e => e.LangCode).HasMaxLength(500);
        builder.Property(e => e.LangText).HasMaxLength(500);
        builder.Property(e => e.LangType).HasMaxLength(50);
        builder.HasIndex(e => new { e.LangCode, e.LangType }).IsUnique();
    }
}
