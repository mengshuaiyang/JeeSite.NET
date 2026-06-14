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

// 定义class RoleFieldScopeConfiguration
// 定义类：RoleFieldScopeConfiguration
public class RoleFieldScopeConfiguration : IEntityTypeConfiguration<RoleFieldScope>
{
    // 方法 Configure
    // 方法：Configure
    public void Configure(EntityTypeBuilder<RoleFieldScope> builder)
    {
        builder.ToTable("Sys_Role_Field_Scope");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasMaxLength(100);
        builder.Property(e => e.RoleCode).HasMaxLength(100);
        builder.Property(e => e.MenuCode).HasMaxLength(100);
        builder.Property(e => e.EntityName).HasMaxLength(200);
        builder.Property(e => e.EntityLabel).HasMaxLength(200);
        builder.Property(e => e.EntityClass).HasMaxLength(500);
        builder.Property(e => e.FieldConfig).HasColumnType("text");
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.HasIndex(e => e.RoleCode);
    }
}
