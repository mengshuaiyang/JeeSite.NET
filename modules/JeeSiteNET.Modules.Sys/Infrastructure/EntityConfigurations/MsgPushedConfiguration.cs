using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations;

public class MsgPushedConfiguration : IEntityTypeConfiguration<MsgPushed>
{
    public void Configure(EntityTypeBuilder<MsgPushed> builder)
    {
        builder.ToTable("Sys_Msg_Pushed");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasMaxLength(100);
        builder.Property(e => e.MsgType).HasMaxLength(16);
        builder.Property(e => e.MsgTitle).HasMaxLength(200);
        builder.Property(e => e.MsgContent).HasColumnType("text");
        builder.Property(e => e.BizKey).HasMaxLength(100);
        builder.Property(e => e.BizType).HasMaxLength(100);
        builder.Property(e => e.ReceiveCode).HasMaxLength(100);
        builder.Property(e => e.ReceiveUserCode).HasMaxLength(100);
        builder.Property(e => e.ReceiveUserName).HasMaxLength(200);
        builder.Property(e => e.SendUserCode).HasMaxLength(100);
        builder.Property(e => e.SendUserName).HasMaxLength(200);
        builder.Property(e => e.IsMergePush).HasMaxLength(1);
        builder.Property(e => e.PushNumber).HasColumnType("int");
        builder.Property(e => e.PushReturnCode).HasMaxLength(200);
        builder.Property(e => e.PushReturnMsgId).HasMaxLength(200);
        builder.Property(e => e.PushReturnContent).HasColumnType("text");
        builder.Property(e => e.PushStatus).HasMaxLength(1);
        builder.Property(e => e.ReadStatus).HasMaxLength(1);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.HasIndex(e => e.MsgType);
        builder.HasIndex(e => e.ReceiveCode);
        builder.HasIndex(e => e.ReceiveUserCode);
        builder.HasIndex(e => e.PushStatus);
        builder.HasIndex(e => e.ReadStatus);
        builder.HasIndex(e => new { e.BizType, e.BizKey });
        builder.HasIndex(e => e.IsMergePush);
    }
}
