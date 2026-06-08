using JeeSiteNET.Modules.Bpm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Bpm.Infrastructure.EntityConfigurations;

public class LeaveRequestConfiguration : IEntityTypeConfiguration<LeaveRequest>
{
    public void Configure(EntityTypeBuilder<LeaveRequest> builder)
    {
        builder.ToTable("Bpm_LeaveRequest");
        builder.HasKey(e => e.LeaveRequestId);
        builder.Property(e => e.LeaveRequestId).HasMaxLength(64);
        builder.Property(e => e.Applicant).HasMaxLength(64).IsRequired();
        builder.Property(e => e.LeaveType).HasMaxLength(32).IsRequired();
        builder.Property(e => e.Reason).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(32).IsRequired();
        builder.Property(e => e.ManagerApprover).HasMaxLength(64);
        builder.Property(e => e.HrApprover).HasMaxLength(64);
        builder.Property(e => e.CreateBy).HasMaxLength(64);
        builder.Property(e => e.UpdateBy).HasMaxLength(64);
        builder.HasIndex(e => e.Applicant);
        builder.HasIndex(e => e.Status);
    }
}
