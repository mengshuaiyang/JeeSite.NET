using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations;

public class MsgInnerConfiguration : IEntityTypeConfiguration<MsgInner>
{
    public void Configure(EntityTypeBuilder<MsgInner> builder)
    {
        builder.ToTable("Sys_Msg_Inner");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasMaxLength(100);
        builder.Property(e => e.MsgTitle).HasMaxLength(200).IsRequired();
        builder.Property(e => e.ContentLevel).HasMaxLength(1).IsRequired();
        builder.Property(e => e.ContentType).HasMaxLength(1);
        builder.Property(e => e.MsgContent).HasColumnType("text").IsRequired();
        builder.Property(e => e.ReceiveType).HasMaxLength(1).IsRequired();
        builder.Property(e => e.ReceiveCodes).HasColumnType("text");
        builder.Property(e => e.ReceiveNames).HasColumnType("text");
        builder.Property(e => e.SendUserCode).HasMaxLength(100);
        builder.Property(e => e.SendUserName).HasMaxLength(200);
        builder.Property(e => e.IsAttac).HasMaxLength(1);
        builder.Property(e => e.NotifyTypes).HasMaxLength(200);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.HasIndex(e => e.CreateBy);
        builder.HasIndex(e => e.SendUserCode);
        builder.HasIndex(e => e.SendDate);
    }
}
