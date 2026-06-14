    // 引入 JeeSiteNET.Modules.App.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.App.Domain.Entities
using JeeSiteNET.Modules.App.Domain.Entities;
    // 引入 Microsoft.EntityFrameworkCore 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore
using Microsoft.EntityFrameworkCore;
    // 引入 Microsoft.EntityFrameworkCore.Metadata.Builders 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore.Metadata.Builders
using Microsoft.EntityFrameworkCore.Metadata.Builders;

// 定义 JeeSiteNET.Modules.App.Infrastructure.EntityConfigurations 命名空间
// 定义命名空间：JeeSiteNET.Modules.App.Infrastructure.EntityConfigurations
namespace JeeSiteNET.Modules.App.Infrastructure.EntityConfigurations;

// 定义class AppUpgradeConfiguration
// 定义类：AppUpgradeConfiguration
public class AppUpgradeConfiguration : IEntityTypeConfiguration<AppUpgrade>
{
    // 方法 Configure
    // 方法：Configure
    public void Configure(EntityTypeBuilder<AppUpgrade> builder)
    {
        builder.ToTable("App_Upgrade");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasMaxLength(100);
        builder.Property(e => e.AppCode).HasMaxLength(100);
        builder.Property(e => e.UpTitle).HasMaxLength(200);
        builder.Property(e => e.UpContent).HasMaxLength(1000);
        builder.Property(e => e.UpType).HasMaxLength(1);
        builder.Property(e => e.ApkUrl).HasMaxLength(500);
        builder.Property(e => e.ResUrl).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
    }
}
