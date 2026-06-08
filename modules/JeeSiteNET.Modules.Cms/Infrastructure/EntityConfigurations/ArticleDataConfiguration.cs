using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using JeeSiteNET.Modules.Cms.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Cms.Infrastructure.EntityConfigurations;

public class ArticleDataConfiguration : IEntityTypeConfiguration<ArticleData>
{
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
