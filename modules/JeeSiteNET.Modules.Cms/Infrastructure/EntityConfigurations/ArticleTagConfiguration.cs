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

// 定义class ArticleTagConfiguration
// 定义类：ArticleTagConfiguration
public class ArticleTagConfiguration : IEntityTypeConfiguration<ArticleTag>
{
    // 方法 Configure
    // 方法：Configure
    public void Configure(EntityTypeBuilder<ArticleTag> builder)
    {
        builder.ToTable("Cms_Article_Tag");
        builder.HasKey(e => new { e.ArticleCode, e.TagName });
        builder.Property(e => e.ArticleCode).HasMaxLength(100);
        builder.Property(e => e.TagName).HasMaxLength(200);
        builder.HasIndex(e => e.ArticleCode);
        builder.HasOne(e => e.Article).WithMany(e => e.ArticleTags).HasForeignKey(e => e.ArticleCode);
        builder.HasOne(e => e.Tag).WithMany(e => e.ArticleTags).HasForeignKey(e => e.TagName);
    }
}
