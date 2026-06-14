    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;
    // 引入 Microsoft.EntityFrameworkCore 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore
using Microsoft.EntityFrameworkCore;
    // 引入 Microsoft.EntityFrameworkCore.Metadata.Builders 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore.Metadata.Builders
using Microsoft.EntityFrameworkCore.Metadata.Builders;

// 定义 JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations
namespace JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations;

// 定义class AuditConfiguration
// 定义类：AuditConfiguration
public class AuditConfiguration : IEntityTypeConfiguration<Audit>
{
    // 方法 Configure
    // 方法：Configure
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
