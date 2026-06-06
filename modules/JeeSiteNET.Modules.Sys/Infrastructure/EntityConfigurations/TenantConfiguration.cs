using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("Sys_Tenant");
        builder.HasKey(e => e.TenantCode);
        builder.Property(e => e.TenantCode).HasMaxLength(100);
        builder.Property(e => e.TenantName).HasMaxLength(200);
        builder.Property(e => e.ContactName).HasMaxLength(100);
        builder.Property(e => e.ContactPhone).HasMaxLength(50);
        builder.Property(e => e.ExpireDate).HasMaxLength(10);
        builder.Property(e => e.IsAvailable).HasMaxLength(1);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(1);
    }
}
