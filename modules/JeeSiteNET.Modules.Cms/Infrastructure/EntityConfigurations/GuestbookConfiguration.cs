using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using JeeSiteNET.Modules.Cms.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Cms.Infrastructure.EntityConfigurations;

public class GuestbookConfiguration : IEntityTypeConfiguration<Guestbook>
{
    public void Configure(EntityTypeBuilder<Guestbook> builder)
    {
        builder.ToTable("Cms_Guestbook");
        builder.HasKey(e => e.GbCode);
        builder.Property(e => e.GbCode).HasMaxLength(100);
        builder.Property(e => e.GbType).HasMaxLength(1);
        builder.Property(e => e.Content).HasMaxLength(500);
        builder.Property(e => e.Name).HasMaxLength(100);
        builder.Property(e => e.Email).HasMaxLength(100);
        builder.Property(e => e.Phone).HasMaxLength(100);
        builder.Property(e => e.WorkUnit).HasMaxLength(100);
        builder.Property(e => e.Ip).HasMaxLength(100);
        builder.Property(e => e.ReUserCode).HasMaxLength(100);
        builder.Property(e => e.ReContent).HasMaxLength(500);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.ConfigureCorpFields();
        builder.HasIndex(e => e.GbType);
    }
}
