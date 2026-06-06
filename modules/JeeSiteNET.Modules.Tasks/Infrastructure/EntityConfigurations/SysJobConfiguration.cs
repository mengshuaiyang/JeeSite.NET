using JeeSiteNET.Modules.Tasks.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Tasks.Infrastructure.EntityConfigurations;

public class SysJobConfiguration : IEntityTypeConfiguration<SysJob>
{
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
