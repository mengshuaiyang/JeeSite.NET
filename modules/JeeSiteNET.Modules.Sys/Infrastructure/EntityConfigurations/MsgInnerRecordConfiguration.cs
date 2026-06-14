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

// 定义class MsgInnerRecordConfiguration
// 定义类：MsgInnerRecordConfiguration
public class MsgInnerRecordConfiguration : IEntityTypeConfiguration<MsgInnerRecord>
{
    // 方法 Configure
    // 方法：Configure
    public void Configure(EntityTypeBuilder<MsgInnerRecord> builder)
    {
        builder.ToTable("Sys_Msg_Inner_Record");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasMaxLength(100);
        builder.Property(e => e.MsgInnerId).HasMaxLength(100).IsRequired();
        builder.Property(e => e.ReceiveUserCode).HasMaxLength(100).IsRequired();
        builder.Property(e => e.ReceiveUserName).HasMaxLength(200).IsRequired();
        builder.Property(e => e.ReadStatus).HasMaxLength(1).IsRequired();
        builder.Property(e => e.IsStar).HasMaxLength(1);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.HasIndex(e => e.MsgInnerId);
        builder.HasIndex(e => e.ReceiveUserCode);
        builder.HasIndex(e => e.ReadStatus);
    }
}
