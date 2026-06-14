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

// 定义class CommentConfiguration
// 定义类：CommentConfiguration
public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    // 方法 Configure
    // 方法：Configure
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Cms_Comment");
        builder.HasKey(e => e.CommentCode);
        builder.Property(e => e.CommentCode).HasMaxLength(100);
        builder.Property(e => e.CategoryCode).HasMaxLength(100);
        builder.Property(e => e.ArticleCode).HasMaxLength(100);
        builder.Property(e => e.ParentCode).HasMaxLength(100);
        builder.Property(e => e.ArticleTitle).HasMaxLength(500);
        builder.Property(e => e.Content).HasMaxLength(500);
        builder.Property(e => e.Name).HasMaxLength(100);
        builder.Property(e => e.Ip).HasMaxLength(100);
        builder.Property(e => e.AuditUserCode).HasMaxLength(100);
        builder.Property(e => e.AuditComment).HasMaxLength(200);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.ConfigureCorpFields();
        builder.HasIndex(e => e.ArticleCode);
        builder.HasIndex(e => e.CategoryCode);
    }
}
