using JeeSiteNET.Modules.Bpm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Bpm.Infrastructure.EntityConfigurations;

public class ApprovalRecordConfiguration : IEntityTypeConfiguration<ApprovalRecord>
{
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
