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

// 定义class EmployeeConfiguration
// 定义类：EmployeeConfiguration
public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    // 方法 Configure
    // 方法：Configure
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Sys_Employee");
        builder.HasKey(e => e.EmpCode);
        builder.Property(e => e.EmpCode).HasMaxLength(100);
        builder.Property(e => e.EmpNo).HasMaxLength(100);
        builder.Property(e => e.EmpName).HasMaxLength(100);
        builder.Property(e => e.EmpNameEn).HasMaxLength(100);
        builder.Property(e => e.OfficeCode).HasMaxLength(100);
        builder.Property(e => e.OfficeName).HasMaxLength(200);
        builder.Property(e => e.CompanyCode).HasMaxLength(100);
        builder.Property(e => e.CompanyName).HasMaxLength(200);
        builder.Property(e => e.CorpCode).HasMaxLength(100);
        builder.Property(e => e.CorpName).HasMaxLength(200);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.Property(e => e.ExtendJson).HasColumnType("text");
        builder.HasIndex(e => e.EmpNo);
        builder.HasIndex(e => e.OfficeCode);
        builder.HasIndex(e => e.CompanyCode);
    }
}
