    // 引入 JeeSiteNET.Modules.Bpm.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Bpm.Domain.Entities
using JeeSiteNET.Modules.Bpm.Domain.Entities;
    // 引入 Microsoft.EntityFrameworkCore 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore
using Microsoft.EntityFrameworkCore;
    // 引入 Microsoft.EntityFrameworkCore.Metadata.Builders 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore.Metadata.Builders
using Microsoft.EntityFrameworkCore.Metadata.Builders;

// 定义 JeeSiteNET.Modules.Bpm.Infrastructure.EntityConfigurations 命名空间
// 定义命名空间：JeeSiteNET.Modules.Bpm.Infrastructure.EntityConfigurations
namespace JeeSiteNET.Modules.Bpm.Infrastructure.EntityConfigurations;

// 定义class WorkflowFormConfiguration
// 定义类：WorkflowFormConfiguration
public class WorkflowFormConfiguration : IEntityTypeConfiguration<WorkflowForm>
{
    // 方法 Configure
    // 方法：Configure
    public void Configure(EntityTypeBuilder<WorkflowForm> builder)
    {
        builder.ToTable("Bpm_WorkflowForm");
        builder.HasKey(e => e.FormId);
        builder.Property(e => e.FormId).HasMaxLength(100);
        builder.Property(e => e.WorkflowInstanceId).HasMaxLength(100);
        builder.Property(e => e.BusinessKey).HasMaxLength(100);
        builder.Property(e => e.BusinessType).HasMaxLength(50);
        builder.Property(e => e.FormData);
        builder.Property(e => e.CurrentActivityId).HasMaxLength(100);
        builder.Property(e => e.CurrentAssignee).HasMaxLength(100);
        builder.Property(e => e.FormStatus).HasColumnName("Status").HasMaxLength(20);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.HasIndex(e => e.BusinessKey);
    }
}
