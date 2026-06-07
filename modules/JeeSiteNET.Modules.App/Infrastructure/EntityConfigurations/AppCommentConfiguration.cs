using JeeSiteNET.Modules.App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.App.Infrastructure.EntityConfigurations;

public class AppCommentConfiguration : IEntityTypeConfiguration<AppComment>
{
    public void Configure(EntityTypeBuilder<AppComment> builder)
    {
        builder.ToTable("App_Comment");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasMaxLength(100);
        builder.Property(e => e.Category).HasMaxLength(10);
        builder.Property(e => e.Content).HasMaxLength(500);
        builder.Property(e => e.Contact).HasMaxLength(200);
        builder.Property(e => e.CreateByName).HasMaxLength(200);
        builder.Property(e => e.DeviceInfo).HasMaxLength(4000);
        builder.Property(e => e.ReplyContent).HasMaxLength(500);
        builder.Property(e => e.ReplyUserCode).HasMaxLength(100);
        builder.Property(e => e.ReplyUserName).HasMaxLength(200);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
    }
}
