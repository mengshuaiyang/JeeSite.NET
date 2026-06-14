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

// 定义class ApprovalRecordConfiguration
// 定义类：ApprovalRecordConfiguration
public class ApprovalRecordConfiguration : IEntityTypeConfiguration<ApprovalRecord>
{
    // 方法 Configure
    // 方法：Configure
    public void Configure(EntityTypeBuilder<ApprovalRecord> builder)
    {
        builder.ToTable("Bpm_ApprovalRecord");
        builder.HasKey(e => e.RecordId);
        builder.Property(e => e.RecordId).HasMaxLength(100);
        builder.Property(e => e.WorkflowInstanceId).HasMaxLength(100);
        builder.Property(e => e.BusinessKey).HasMaxLength(100);
        builder.Property(e => e.BusinessType).HasMaxLength(50);
        builder.Property(e => e.ActivityId).HasMaxLength(100);
        builder.Property(e => e.ActivityName).HasMaxLength(200);
        builder.Property(e => e.Assignee).HasMaxLength(100);
        builder.Property(e => e.AssigneeName).HasMaxLength(100);
        builder.Property(e => e.Result).HasMaxLength(20);
        builder.Property(e => e.Comment).HasMaxLength(2000);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.HasIndex(e => e.BusinessKey);
        builder.HasIndex(e => e.Assignee);
    }
}
