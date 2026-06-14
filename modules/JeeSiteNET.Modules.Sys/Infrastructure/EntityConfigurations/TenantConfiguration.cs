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

// 定义class TenantConfiguration
// 定义类：TenantConfiguration
public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    // 方法 Configure
    // 方法：Configure
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
