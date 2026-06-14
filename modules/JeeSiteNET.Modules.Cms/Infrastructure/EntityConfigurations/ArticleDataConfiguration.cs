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

// 定义class ArticleDataConfiguration
// 定义类：ArticleDataConfiguration
public class ArticleDataConfiguration : IEntityTypeConfiguration<ArticleData>
{
    // 方法 Configure
    // 方法：Configure
    public void Configure(EntityTypeBuilder<ArticleData> builder)
    {
        builder.ToTable("Cms_Article_Data");
        builder.HasKey(e => e.ArticleCode);
        builder.Property(e => e.ArticleCode).HasMaxLength(100);
        builder.Property(e => e.Content);
        builder.Property(e => e.Relation).HasMaxLength(1000);
        builder.Property(e => e.IsCanComment).HasMaxLength(1);
        builder.ConfigureExtendFields();
        builder.HasOne(e => e.Article).WithOne(e => e.ArticleData).HasForeignKey<ArticleData>(e => e.ArticleCode);
    }
}
