using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations;

public class ConfigConfiguration : IEntityTypeConfiguration<Config>
{
    public void Configure(EntityTypeBuilder<Config> builder)
    {
        builder.ToTable("Sys_Config");
        builder.HasKey(e => e.ConfigKey);
        builder.Property(e => e.ConfigKey).HasMaxLength(100);
        builder.Property(e => e.ConfigName).HasMaxLength(200);
        builder.Property(e => e.ConfigValue).HasMaxLength(2000);
        builder.Property(e => e.IsSys).HasMaxLength(1);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.HasIndex(e => e.ConfigName);
    }
}
