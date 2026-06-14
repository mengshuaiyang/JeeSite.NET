    // 引入 JeeSiteNET.Modules.CodeGen.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.CodeGen.Domain.Entities
using JeeSiteNET.Modules.CodeGen.Domain.Entities;
    // 引入 Microsoft.EntityFrameworkCore 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore
using Microsoft.EntityFrameworkCore;
    // 引入 Microsoft.EntityFrameworkCore.Metadata.Builders 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore.Metadata.Builders
using Microsoft.EntityFrameworkCore.Metadata.Builders;

// 定义 JeeSiteNET.Modules.CodeGen.Infrastructure.EntityConfigurations 命名空间
// 定义命名空间：JeeSiteNET.Modules.CodeGen.Infrastructure.EntityConfigurations
namespace JeeSiteNET.Modules.CodeGen.Infrastructure.EntityConfigurations;

// 定义class GenTableColumnConfiguration
// 定义类：GenTableColumnConfiguration
public class GenTableColumnConfiguration : IEntityTypeConfiguration<GenTableColumn>
{
    // 方法 Configure
    // 方法：Configure
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
