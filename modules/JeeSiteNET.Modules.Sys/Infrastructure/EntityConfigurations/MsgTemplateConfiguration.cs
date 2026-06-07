using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations;

public class MsgTemplateConfiguration : IEntityTypeConfiguration<MsgTemplate>
{
    public void Configure(EntityTypeBuilder<MsgTemplate> builder)
    {
        builder.ToTable("Sys_Msg_Template");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasMaxLength(100);
        builder.Property(e => e.ModuleCode).HasMaxLength(100);
        builder.Property(e => e.TplKey).HasMaxLength(100);
        builder.Property(e => e.TplName).HasMaxLength(200);
        builder.Property(e => e.TplType).HasMaxLength(16);
        builder.Property(e => e.TplContent).HasColumnType("text");
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.HasIndex(e => e.TplKey);
        builder.HasIndex(e => e.TplType);
        builder.HasIndex(e => e.Status);
    }
}
