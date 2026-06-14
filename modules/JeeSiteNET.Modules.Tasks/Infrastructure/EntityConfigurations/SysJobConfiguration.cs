    // 引入 JeeSiteNET.Modules.Tasks.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Tasks.Domain.Entities
using JeeSiteNET.Modules.Tasks.Domain.Entities;
    // 引入 Microsoft.EntityFrameworkCore 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore
using Microsoft.EntityFrameworkCore;
    // 引入 Microsoft.EntityFrameworkCore.Metadata.Builders 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore.Metadata.Builders
using Microsoft.EntityFrameworkCore.Metadata.Builders;

// 定义 JeeSiteNET.Modules.Tasks.Infrastructure.EntityConfigurations 命名空间
// 定义命名空间：JeeSiteNET.Modules.Tasks.Infrastructure.EntityConfigurations
namespace JeeSiteNET.Modules.Tasks.Infrastructure.EntityConfigurations;

// 定义class SysJobConfiguration
// 定义类：SysJobConfiguration
public class SysJobConfiguration : IEntityTypeConfiguration<SysJob>
{
    // 方法 Configure
    // 方法：Configure
    public void Configure(EntityTypeBuilder<SysJob> builder)
    {
        builder.ToTable("Tasks_Job");
        builder.HasKey(e => e.JobId);
        builder.Property(e => e.JobId).HasMaxLength(100);
        builder.Property(e => e.JobName).HasMaxLength(200);
        builder.Property(e => e.JobGroup).HasMaxLength(50);
        builder.Property(e => e.CronExpression).HasMaxLength(100);
        builder.Property(e => e.AssemblyName).HasMaxLength(500);
        builder.Property(e => e.ClassName).HasMaxLength(500);
        builder.Property(e => e.Description).HasMaxLength(500);
        builder.Property(e => e.RunStatus).HasMaxLength(1);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(1);
    }
}
