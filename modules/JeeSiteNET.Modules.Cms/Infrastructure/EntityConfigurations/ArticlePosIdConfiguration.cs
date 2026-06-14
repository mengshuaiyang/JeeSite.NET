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

// 定义class ArticlePosIdConfiguration
// 定义类：ArticlePosIdConfiguration
public class ArticlePosIdConfiguration : IEntityTypeConfiguration<ArticlePosId>
{
    // 方法 Configure
    // 方法：Configure
    public void Configure(EntityTypeBuilder<ArticlePosId> builder)
    {
        builder.ToTable("Cms_Article_PosId");
        builder.HasKey(e => new { e.ArticleCode, e.PosId });
        builder.Property(e => e.ArticleCode).HasMaxLength(100);
        builder.Property(e => e.PosId).HasMaxLength(1);
        builder.HasIndex(e => e.ArticleCode);
        builder.HasOne(e => e.Article).WithMany(e => e.PosIds).HasForeignKey(e => e.ArticleCode);
    }
}
