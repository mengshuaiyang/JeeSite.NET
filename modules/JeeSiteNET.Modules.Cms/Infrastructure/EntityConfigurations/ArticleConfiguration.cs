using JeeSiteNET.Modules.Cms.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Cms.Infrastructure.EntityConfigurations;

public class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.ToTable("Cms_Article");
        builder.HasKey(e => e.ArticleCode);
        builder.Property(e => e.ArticleCode).HasMaxLength(100);
        builder.Property(e => e.CategoryCode).HasMaxLength(100);
        builder.Property(e => e.Title).HasMaxLength(500);
        builder.Property(e => e.Subtitle).HasMaxLength(500);
        builder.Property(e => e.Summary).HasMaxLength(2000);
        builder.Property(e => e.Content).HasColumnType("nvarchar(max)");
        builder.Property(e => e.Author).HasMaxLength(100);
        builder.Property(e => e.Source).HasMaxLength(200);
        builder.Property(e => e.Image).HasMaxLength(500);
        builder.Property(e => e.Tags).HasMaxLength(500);
        builder.Property(e => e.IsTop).HasMaxLength(1);
        builder.Property(e => e.IsRecommend).HasMaxLength(1);
        builder.Property(e => e.IsHot).HasMaxLength(1);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.HasIndex(e => e.CategoryCode);
        builder.HasIndex(e => e.PublishDate);
    }
}
