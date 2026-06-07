using JeeSiteNET.Modules.App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.App.Infrastructure.EntityConfigurations;

public class AppUpgradeConfiguration : IEntityTypeConfiguration<AppUpgrade>
{
    public void Configure(EntityTypeBuilder<AppUpgrade> builder)
    {
        builder.ToTable("App_Upgrade");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasMaxLength(100);
        builder.Property(e => e.AppCode).HasMaxLength(100);
        builder.Property(e => e.UpTitle).HasMaxLength(200);
        builder.Property(e => e.UpContent).HasMaxLength(1000);
        builder.Property(e => e.UpType).HasMaxLength(1);
        builder.Property(e => e.ApkUrl).HasMaxLength(500);
        builder.Property(e => e.ResUrl).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
    }
}
