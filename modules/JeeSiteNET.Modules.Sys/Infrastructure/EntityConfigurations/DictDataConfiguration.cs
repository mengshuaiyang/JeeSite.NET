using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations;

public class DictDataConfiguration : IEntityTypeConfiguration<DictData>
{
    public void Configure(EntityTypeBuilder<DictData> builder)
    {
        builder.ToTable("Sys_Dict_Data");
        builder.HasKey(e => e.DictCode);
        builder.Property(e => e.DictCode).HasMaxLength(100);
        builder.Property(e => e.DictType).HasMaxLength(100);
        builder.Property(e => e.DictLabel).HasMaxLength(200);
        builder.Property(e => e.DictValue).HasMaxLength(500);
        builder.Property(e => e.DictIcon).HasMaxLength(100);
        builder.Property(e => e.Description).HasMaxLength(500);
        builder.Property(e => e.CssStyle).HasMaxLength(200);
        builder.Property(e => e.CssClass).HasMaxLength(200);
        builder.Property(e => e.Sort).HasColumnType("decimal(10,2)");
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.ConfigureCorpFields();
        builder.ConfigureExtendFields();
        builder.HasIndex(e => e.DictType);
    }
}
