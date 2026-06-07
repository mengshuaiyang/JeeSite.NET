using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations;

public class BizCategoryConfiguration : IEntityTypeConfiguration<BizCategory>
{
    public void Configure(EntityTypeBuilder<BizCategory> builder)
    {
        builder.ToTable("Biz_Category");
        builder.HasKey(e => e.CategoryCode);
        builder.Property(e => e.CategoryCode).HasMaxLength(100);
        builder.Property(e => e.ViewCode).HasMaxLength(500);
        builder.Property(e => e.CategoryName).HasMaxLength(100);
        builder.Property(e => e.ParentCode).HasMaxLength(100);
        builder.Property(e => e.ParentCodes).HasMaxLength(767);
        builder.Property(e => e.TreeSorts).HasMaxLength(767);
        builder.Property(e => e.TreeNames).HasMaxLength(767);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.ConfigureCorpFields();
        builder.HasIndex(e => e.ViewCode);
        builder.HasIndex(e => e.ParentCode);
        builder.HasIndex(e => e.TreeSort);
    }
}
