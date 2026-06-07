using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using JeeSiteNET.Modules.Cms.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Cms.Infrastructure.EntityConfigurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Cms_Category");
        builder.HasKey(e => e.CategoryCode);
        builder.Property(e => e.CategoryCode).HasMaxLength(100);
        builder.Property(e => e.CategoryName).HasMaxLength(200);
        builder.Property(e => e.CategoryType).HasMaxLength(20);
        builder.Property(e => e.Image).HasMaxLength(500);
        builder.Property(e => e.Link).HasMaxLength(500);
        builder.Property(e => e.Target).HasMaxLength(20);
        builder.Property(e => e.Keywords).HasMaxLength(500);
        builder.Property(e => e.Description).HasMaxLength(500);
        builder.Property(e => e.InMenu).HasMaxLength(1);
        builder.Property(e => e.InList).HasMaxLength(1);
        builder.Property(e => e.ShowModes).HasMaxLength(500);
        builder.Property(e => e.IsShow).HasMaxLength(1);
        builder.Property(e => e.IsNeedAudit).HasMaxLength(1);
        builder.Property(e => e.IsCanComment).HasMaxLength(1);
        builder.Property(e => e.SiteCode).HasMaxLength(100);
        builder.Property(e => e.CustomListView).HasMaxLength(500);
        builder.Property(e => e.CustomContentView).HasMaxLength(500);
        builder.Property(e => e.ViewConfig).HasMaxLength(500);
        builder.Property(e => e.ParentCode).HasMaxLength(100);
        builder.Property(e => e.ParentCodes).HasMaxLength(2000);
        builder.Property(e => e.TreeSort).HasColumnType("decimal(10,2)");
        builder.Property(e => e.TreeLevel).HasColumnType("decimal(10,2)");
        builder.Property(e => e.TreeSorts).HasMaxLength(2000);
        builder.Property(e => e.TreeLeaf).HasMaxLength(1);
        builder.Property(e => e.TreeNames).HasMaxLength(2000);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.HasIndex(e => e.ParentCode);
        builder.HasIndex(e => e.SiteCode);
        builder.ConfigureCorpFields();
        builder.ConfigureExtendFields();
    }
}
