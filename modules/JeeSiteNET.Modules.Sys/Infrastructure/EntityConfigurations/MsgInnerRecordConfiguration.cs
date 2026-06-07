using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations;

public class MsgInnerRecordConfiguration : IEntityTypeConfiguration<MsgInnerRecord>
{
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
