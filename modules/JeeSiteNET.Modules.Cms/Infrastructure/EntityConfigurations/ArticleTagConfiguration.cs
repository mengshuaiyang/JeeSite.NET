using JeeSiteNET.Modules.Cms.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Cms.Infrastructure.EntityConfigurations;

public class ArticleTagConfiguration : IEntityTypeConfiguration<ArticleTag>
{
    public void Configure(EntityTypeBuilder<ArticleTag> builder)
    {
        builder.ToTable("Cms_Article_Tag");
        builder.HasKey(e => new { e.ArticleCode, e.TagName });
        builder.Property(e => e.ArticleCode).HasMaxLength(100);
        builder.Property(e => e.TagName).HasMaxLength(200);
        builder.HasOne(e => e.Article).WithMany(e => e.ArticleTags).HasForeignKey(e => e.ArticleCode);
        builder.HasOne(e => e.Tag).WithMany(e => e.ArticleTags).HasForeignKey(e => e.TagName);
    }
}
