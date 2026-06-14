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

// 定义class MenuConfiguration
// 定义类：MenuConfiguration
public class MenuConfiguration : IEntityTypeConfiguration<Menu>
{
    // 方法 Configure
    // 方法：Configure
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.ToTable("Sys_Menu");
        builder.HasKey(e => e.MenuCode);
        builder.Property(e => e.MenuCode).HasMaxLength(100);
        builder.Property(e => e.MenuName).HasMaxLength(200);
        builder.Property(e => e.MenuType).HasMaxLength(1);
        builder.Property(e => e.MenuHref).HasMaxLength(1000);
        builder.Property(e => e.MenuTarget).HasMaxLength(20);
        builder.Property(e => e.MenuIcon).HasMaxLength(100);
        builder.Property(e => e.MenuColor).HasMaxLength(20);
        builder.Property(e => e.MenuTitle).HasMaxLength(200);
        builder.Property(e => e.Permission).HasMaxLength(500);
        builder.Property(e => e.IsShow).HasMaxLength(1);
        builder.Property(e => e.SysCode).HasMaxLength(100);
        builder.Property(e => e.ModuleCodes).HasMaxLength(500);
        builder.Property(e => e.ModuleCode).HasMaxLength(100);
        builder.Property(e => e.Component).HasMaxLength(500);
        builder.Property(e => e.Params).HasMaxLength(500);
        builder.Property(e => e.Weight).HasColumnType("decimal(10,2)");
        builder.Property(e => e.TreeSort).HasColumnType("decimal(10,2)");
        builder.Property(e => e.TreeLevel).HasColumnType("decimal(10,2)");
        builder.Property(e => e.ParentCode).HasMaxLength(100);
        builder.Property(e => e.ParentCodes).HasMaxLength(2000);
        builder.Property(e => e.TreeSorts).HasMaxLength(2000);
        builder.Property(e => e.TreeLeaf).HasMaxLength(1);
        builder.Property(e => e.TreeNames).HasMaxLength(2000);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.ConfigureCorpFields();
        builder.ConfigureExtendFields();
        builder.HasIndex(e => e.ParentCode);
    }
}
