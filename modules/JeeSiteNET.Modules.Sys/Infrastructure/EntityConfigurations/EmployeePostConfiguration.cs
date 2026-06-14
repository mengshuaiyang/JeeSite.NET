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

// 定义class EmployeePostConfiguration
// 定义类：EmployeePostConfiguration
public class EmployeePostConfiguration : IEntityTypeConfiguration<EmployeePost>
{
    // 方法 Configure
    // 方法：Configure
    public void Configure(EntityTypeBuilder<EmployeePost> builder)
    {
        builder.ToTable("Sys_Employee_Post");
        builder.HasKey(e => new { e.EmpCode, e.PostCode });
        builder.Property(e => e.EmpCode).HasMaxLength(100);
        builder.Property(e => e.PostCode).HasMaxLength(100);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.HasIndex(e => e.EmpCode);
        builder.HasIndex(e => e.PostCode);
    }
}
