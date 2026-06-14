    // 引入 JeeSiteNET.Infrastructure.EntityFrameworkCore 命名空间
// 引入命名空间：JeeSiteNET.Infrastructure.EntityFrameworkCore
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
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

// 定义class LogConfiguration
// 定义类：LogConfiguration
public class LogConfiguration : IEntityTypeConfiguration<Log>
{
    // 方法 Configure
    // 方法：Configure
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
        builder.Property(e => e.CreateByName).HasMaxLength(200);
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
