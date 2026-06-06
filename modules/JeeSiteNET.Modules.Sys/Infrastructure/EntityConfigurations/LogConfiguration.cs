using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations;

public class LogConfiguration : IEntityTypeConfiguration<Log>
{
    public void Configure(EntityTypeBuilder<Log> builder)
    {
        builder.ToTable("Sys_Log");
        builder.HasKey(e => e.LogId);
        builder.Property(e => e.LogId).HasMaxLength(100);
        builder.Property(e => e.LogType).HasMaxLength(100);
        builder.Property(e => e.LogTitle).HasMaxLength(500);
        builder.Property(e => e.RequestUri).HasMaxLength(500);
        builder.Property(e => e.RequestMethod).HasMaxLength(10);
        builder.Property(e => e.ExecuteTime).HasColumnType("decimal(18,4)");
        builder.Property(e => e.Params).HasColumnType("text");
        builder.Property(e => e.DiffData).HasColumnType("text");
        builder.Property(e => e.BizKey).HasMaxLength(500);
        builder.Property(e => e.BizType).HasMaxLength(100);
        builder.Property(e => e.UserCode).HasMaxLength(100);
        builder.Property(e => e.UserName).HasMaxLength(200);
        builder.Property(e => e.OrgCode).HasMaxLength(100);
        builder.Property(e => e.RemoteIp).HasMaxLength(100);
        builder.Property(e => e.ServerAddr).HasMaxLength(100);
        builder.Property(e => e.UserAgent).HasMaxLength(500);
        builder.Property(e => e.DeviceName).HasMaxLength(100);
        builder.Property(e => e.BrowserName).HasMaxLength(100);
        builder.Property(e => e.IsException).HasMaxLength(1);
        builder.Property(e => e.ExceptionInfo).HasColumnType("text");
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.ConfigureCorpFields();
        builder.HasIndex(e => e.LogType);
        builder.HasIndex(e => e.CreateDate);
    }
}
