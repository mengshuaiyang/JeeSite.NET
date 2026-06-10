using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations;

public class AuditConfiguration : IEntityTypeConfiguration<Audit>
{
    public void Configure(EntityTypeBuilder<Audit> builder)
    {
        builder.ToTable("Sys_Audit");
        builder.HasKey(e => e.AuditId);
        builder.Property(e => e.AuditId).HasMaxLength(100);
        builder.Property(e => e.AuditType).HasMaxLength(100);
        builder.Property(e => e.AuditResult).HasMaxLength(100);
        builder.Property(e => e.UserCode).HasMaxLength(100);
        builder.Property(e => e.LoginCode).HasMaxLength(100);
        builder.Property(e => e.UserName).HasMaxLength(200);
        builder.Property(e => e.OfficeCode).HasMaxLength(100);
        builder.Property(e => e.OfficeName).HasMaxLength(200);
        builder.Property(e => e.MenuCode).HasMaxLength(100);
        builder.Property(e => e.PwdSecurityLevel).HasMaxLength(1);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.HasIndex(e => e.AuditType);
        builder.HasIndex(e => e.CreateDate);
        builder.HasIndex(e => e.UserCode);
    }
}
