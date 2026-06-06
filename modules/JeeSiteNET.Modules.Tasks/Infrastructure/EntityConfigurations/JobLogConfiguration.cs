using JeeSiteNET.Modules.Tasks.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Tasks.Infrastructure.EntityConfigurations;

public class JobLogConfiguration : IEntityTypeConfiguration<JobLog>
{
    public void Configure(EntityTypeBuilder<JobLog> builder)
    {
        builder.ToTable("Tasks_JobLog");
        builder.HasKey(e => e.LogId);
        builder.Property(e => e.LogId).HasMaxLength(100);
        builder.Property(e => e.JobId).HasMaxLength(100);
        builder.Property(e => e.JobName).HasMaxLength(200);
        builder.Property(e => e.JobGroup).HasMaxLength(50);
        builder.Property(e => e.Result).HasMaxLength(20);
        builder.Property(e => e.ErrorMessage).HasMaxLength(4000);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.HasIndex(e => e.JobId);
    }
}
