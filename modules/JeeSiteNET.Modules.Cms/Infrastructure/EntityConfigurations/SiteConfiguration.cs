using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using JeeSiteNET.Modules.Cms.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Cms.Infrastructure.EntityConfigurations;

public class SiteConfiguration : IEntityTypeConfiguration<Site>
{
    public void Configure(EntityTypeBuilder<Site> builder)
    {
        builder.ToTable("Cms_Site");
        builder.HasKey(e => e.SiteCode);
        builder.Property(e => e.SiteCode).HasMaxLength(100);
        builder.Property(e => e.SiteName).HasMaxLength(200);
        builder.Property(e => e.SiteSort);
        builder.Property(e => e.Title).HasMaxLength(500);
        builder.Property(e => e.Logo).HasMaxLength(500);
        builder.Property(e => e.Domain).HasMaxLength(200);
        builder.Property(e => e.Keywords).HasMaxLength(500);
        builder.Property(e => e.Description).HasMaxLength(500);
        builder.Property(e => e.Theme).HasMaxLength(100);
        builder.Property(e => e.Copyright).HasMaxLength(500);
        builder.Property(e => e.CustomIndexView).HasMaxLength(500);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.ConfigureCorpFields();
    }
}
