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

// 定义class RoleDataScopeConfiguration
// 定义类：RoleDataScopeConfiguration
public class RoleDataScopeConfiguration : IEntityTypeConfiguration<RoleDataScope>
{
    // 方法 Configure
    // 方法：Configure
    public void Configure(EntityTypeBuilder<RoleDataScope> builder)
    {
        builder.ToTable("Sys_Role_Data_Scope");
        builder.HasKey(e => new { e.RoleCode, e.MenuCode });
        builder.Property(e => e.RoleCode).HasMaxLength(100);
        builder.Property(e => e.MenuCode).HasMaxLength(100);
        builder.Property(e => e.RuleName).HasMaxLength(200);
        builder.Property(e => e.RuleType).HasMaxLength(50);
        builder.Property(e => e.RuleConfig).HasColumnType("text");
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.HasIndex(e => e.RoleCode);
    }
}
