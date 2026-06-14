    // 引入 Microsoft.EntityFrameworkCore 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore
using Microsoft.EntityFrameworkCore;
    // 引入 Microsoft.EntityFrameworkCore.Metadata.Builders 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore.Metadata.Builders
using Microsoft.EntityFrameworkCore.Metadata.Builders;
    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;

// 定义 JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations
namespace JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations;

// 定义class EmpUserConfiguration
// 定义类：EmpUserConfiguration
public class EmpUserConfiguration : IEntityTypeConfiguration<EmpUser>
{
    // 方法 Configure
    // 方法：Configure
    public void Configure(EntityTypeBuilder<EmpUser> builder)
    {
        builder.ToTable("Sys_EmpUser");
        builder.HasKey(e => new { e.EmpCode, e.UserCode });
        builder.Property(e => e.EmpCode).HasMaxLength(100).IsRequired();
        builder.Property(e => e.UserCode).HasMaxLength(100).IsRequired();
        builder.Property(e => e.EmpName).HasMaxLength(200);
        builder.Property(e => e.LoginCode).HasMaxLength(100);
        builder.Property(e => e.UserName).HasMaxLength(200);
        builder.HasIndex(e => e.EmpCode);
        builder.HasIndex(e => e.UserCode).IsUnique();
    }
}
