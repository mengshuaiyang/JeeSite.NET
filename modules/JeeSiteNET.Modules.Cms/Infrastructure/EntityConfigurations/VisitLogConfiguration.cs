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

// 定义class VisitLogConfiguration
// 定义类：VisitLogConfiguration
public class VisitLogConfiguration : IEntityTypeConfiguration<VisitLog>
{
    // 方法 Configure
    // 方法：Configure
    public void Configure(EntityTypeBuilder<VisitLog> builder)
    {
        builder.ToTable("Cms_Visit_Log");
        builder.HasKey(e => e.VisitId);
        builder.Property(e => e.VisitId).HasMaxLength(100);
        builder.Property(e => e.RequestUrl).HasMaxLength(1000);
        builder.Property(e => e.RequestUrlHost).HasMaxLength(128);
        builder.Property(e => e.SourceReferer).HasMaxLength(1000);
        builder.Property(e => e.SourceRefererHost).HasMaxLength(128);
        builder.Property(e => e.SourceType).HasMaxLength(1);
        builder.Property(e => e.SearchEngine).HasMaxLength(200);
        builder.Property(e => e.SearchWord).HasMaxLength(200);
        builder.Property(e => e.RemoteAddr).HasMaxLength(50);
        builder.Property(e => e.UserAgent).HasMaxLength(500);
        builder.Property(e => e.UserLanguage).HasMaxLength(32);
        builder.Property(e => e.UserScreenSize).HasMaxLength(32);
        builder.Property(e => e.UserDevice).HasMaxLength(32);
        builder.Property(e => e.UserOsName).HasMaxLength(32);
        builder.Property(e => e.UserBrowser).HasMaxLength(32);
        builder.Property(e => e.UserBrowserVersion).HasMaxLength(16);
        builder.Property(e => e.UniqueVisitId).HasMaxLength(100);
        builder.Property(e => e.VisitDate).HasMaxLength(8);
        builder.Property(e => e.IsNewVisit).HasMaxLength(1);
        builder.Property(e => e.SiteCode).HasMaxLength(100);
        builder.Property(e => e.SiteName).HasMaxLength(200);
        builder.Property(e => e.CategoryCode).HasMaxLength(100);
        builder.Property(e => e.CategoryName).HasMaxLength(200);
        builder.Property(e => e.ContentId).HasMaxLength(100);
        builder.Property(e => e.ContentTitle).HasMaxLength(500);
        builder.Property(e => e.VisitUserCode).HasMaxLength(100);
        builder.Property(e => e.VisitUserName).HasMaxLength(100);
        builder.ConfigureCorpFields();
        builder.HasIndex(e => e.CategoryCode);
        builder.HasIndex(e => e.ContentId);
        builder.HasIndex(e => e.VisitDate);
    }
}
