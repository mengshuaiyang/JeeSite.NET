    // 引入 JeeSiteNET.Infrastructure.EntityFrameworkCore 命名空间
// 引入命名空间：JeeSiteNET.Infrastructure.EntityFrameworkCore
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
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

// 定义class RoleConfiguration
// 定义类：RoleConfiguration
public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    // 方法 Configure
    // 方法：Configure
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Sys_Role");
        builder.HasKey(e => e.RoleCode);
        builder.Property(e => e.RoleCode).HasMaxLength(100);
        builder.Property(e => e.RoleName).HasMaxLength(200);
        builder.Property(e => e.ViewCode).HasMaxLength(100);
        builder.Property(e => e.RoleType).HasMaxLength(100);
        builder.Property(e => e.RoleSort).HasColumnType("decimal(10,2)");
        builder.Property(e => e.IsSys).HasMaxLength(1);
        builder.Property(e => e.IsShow).HasMaxLength(1);
        builder.Property(e => e.UserType).HasMaxLength(100);
        builder.Property(e => e.DesktopUrl).HasMaxLength(500);
        builder.Property(e => e.DataScope).HasMaxLength(20);
        builder.Property(e => e.BizScope).HasMaxLength(500);
        builder.Property(e => e.SysCodes).HasMaxLength(500);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.ConfigureCorpFields();
        builder.ConfigureExtendFields();
    }
}
