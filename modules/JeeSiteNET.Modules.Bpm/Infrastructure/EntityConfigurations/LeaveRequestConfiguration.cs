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

// 定义class LeaveRequestConfiguration
// 定义类：LeaveRequestConfiguration
public class LeaveRequestConfiguration : IEntityTypeConfiguration<LeaveRequest>
{
    // 方法 Configure
    // 方法：Configure
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
