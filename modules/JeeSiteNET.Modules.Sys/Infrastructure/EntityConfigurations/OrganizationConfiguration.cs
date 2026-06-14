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

// 定义class OrganizationConfiguration
// 定义类：OrganizationConfiguration
public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    // 方法 Configure
    // 方法：Configure
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.ToTable("Sys_Organization");
        builder.HasKey(e => e.OrgCode);
        builder.Property(e => e.OrgCode).HasMaxLength(100);
        builder.Property(e => e.OrgName).HasMaxLength(200);
        builder.Property(e => e.ViewCode).HasMaxLength(100);
        builder.Property(e => e.FullName).HasMaxLength(200);
        builder.Property(e => e.OrgType).HasMaxLength(100);
        builder.Property(e => e.Leader).HasMaxLength(100);
        builder.Property(e => e.Phone).HasMaxLength(100);
        builder.Property(e => e.Address).HasMaxLength(300);
        builder.Property(e => e.ZipCode).HasMaxLength(100);
        builder.Property(e => e.Email).HasMaxLength(200);
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
        builder.HasIndex(e => e.ParentCodes);
        builder.HasIndex(e => e.TreeSorts);
    }
}
