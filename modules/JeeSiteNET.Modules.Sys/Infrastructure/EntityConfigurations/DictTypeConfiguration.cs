using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations;

public class DictTypeConfiguration : IEntityTypeConfiguration<DictType>
{
    public void Configure(EntityTypeBuilder<DictType> builder)
    {
        builder.ToTable("Sys_Dict_Type");
        builder.HasKey(e => e.DictTypeCode);
        builder.Property(e => e.DictTypeCode).HasMaxLength(100);
        builder.Property(e => e.DictName).HasMaxLength(200);
        builder.Property(e => e.IsSys).HasMaxLength(1);
        builder.Property(e => e.Sort).HasColumnType("decimal(10,2)");
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(1);
    }
}
