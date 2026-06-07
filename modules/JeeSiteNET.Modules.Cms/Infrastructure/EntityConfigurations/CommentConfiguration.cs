using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using JeeSiteNET.Modules.Cms.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Cms.Infrastructure.EntityConfigurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
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
