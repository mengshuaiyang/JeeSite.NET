using JeeSiteNET.Modules.Cms.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Cms.Infrastructure.EntityConfigurations;

public class ArticlePosIdConfiguration : IEntityTypeConfiguration<ArticlePosId>
{
    public void Configure(EntityTypeBuilder<ArticlePosId> builder)
    {
        builder.ToTable("Cms_Article_PosId");
        builder.HasKey(e => new { e.ArticleCode, e.PosId });
        builder.Property(e => e.ArticleCode).HasMaxLength(100);
        builder.Property(e => e.PosId).HasMaxLength(1);
        builder.HasOne(e => e.Article).WithMany(e => e.PosIds).HasForeignKey(e => e.ArticleCode);
    }
}
