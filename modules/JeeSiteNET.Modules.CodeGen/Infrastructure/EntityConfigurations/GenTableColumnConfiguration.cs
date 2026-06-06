using JeeSiteNET.Modules.CodeGen.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.CodeGen.Infrastructure.EntityConfigurations;

public class GenTableColumnConfiguration : IEntityTypeConfiguration<GenTableColumn>
{
    public void Configure(EntityTypeBuilder<GenTableColumn> builder)
    {
        builder.ToTable("CodeGen_TableColumn");
        builder.HasKey(e => e.ColumnId);
        builder.Property(e => e.ColumnId).HasMaxLength(100);
        builder.Property(e => e.TableName).HasMaxLength(100);
        builder.Property(e => e.ColumnName).HasMaxLength(100);
        builder.Property(e => e.ColumnComment).HasMaxLength(500);
        builder.Property(e => e.ColumnType).HasMaxLength(100);
        builder.Property(e => e.NetType).HasMaxLength(50);
        builder.Property(e => e.PropertyName).HasMaxLength(100);
        builder.Property(e => e.IsPk).HasMaxLength(1);
        builder.Property(e => e.IsNullable).HasMaxLength(1);
        builder.Property(e => e.IsInsert).HasMaxLength(1);
        builder.Property(e => e.IsEdit).HasMaxLength(1);
        builder.Property(e => e.IsList).HasMaxLength(1);
        builder.Property(e => e.IsQuery).HasMaxLength(1);
        builder.Property(e => e.QueryType).HasMaxLength(20);
        builder.Property(e => e.HtmlType).HasMaxLength(50);
        builder.Property(e => e.DictType).HasMaxLength(100);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.HasIndex(e => e.TableName);
    }
}
