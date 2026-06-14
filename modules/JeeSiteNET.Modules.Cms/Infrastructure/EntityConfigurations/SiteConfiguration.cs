    // 引入 JeeSiteNET.Infrastructure.EntityFrameworkCore 命名空间
// 引入命名空间：JeeSiteNET.Infrastructure.EntityFrameworkCore
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
    // 引入 JeeSiteNET.Modules.Cms.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Cms.Domain.Entities
using JeeSiteNET.Modules.Cms.Domain.Entities;
    // 引入 Microsoft.EntityFrameworkCore 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore
using Microsoft.EntityFrameworkCore;
    // 引入 Microsoft.EntityFrameworkCore.Metadata.Builders 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore.Metadata.Builders
using Microsoft.EntityFrameworkCore.Metadata.Builders;

// 定义 JeeSiteNET.Modules.Cms.Infrastructure.EntityConfigurations 命名空间
// 定义命名空间：JeeSiteNET.Modules.Cms.Infrastructure.EntityConfigurations
namespace JeeSiteNET.Modules.Cms.Infrastructure.EntityConfigurations;

// 定义class SiteConfiguration
// 定义类：SiteConfiguration
public class SiteConfiguration : IEntityTypeConfiguration<Site>
{
    // 方法 Configure
    // 方法：Configure
    public void Configure(EntityTypeBuilder<Site> builder)
    {
        builder.ToTable("Cms_Site");
        builder.HasKey(e => e.SiteCode);
        builder.Property(e => e.SiteCode).HasMaxLength(100);
        builder.Property(e => e.SiteName).HasMaxLength(200);
        builder.Property(e => e.SiteSort);
        builder.Property(e => e.Title).HasMaxLength(500);
        builder.Property(e => e.Logo).HasMaxLength(500);
        builder.Property(e => e.Domain).HasMaxLength(200);
        builder.Property(e => e.Keywords).HasMaxLength(500);
        builder.Property(e => e.Description).HasMaxLength(500);
        builder.Property(e => e.Theme).HasMaxLength(100);
        builder.Property(e => e.Copyright).HasMaxLength(500);
        builder.Property(e => e.CustomIndexView).HasMaxLength(500);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.ConfigureCorpFields();
    }
}
