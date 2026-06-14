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

// 定义class GenTableConfiguration
// 定义类：GenTableConfiguration
public class GenTableConfiguration : IEntityTypeConfiguration<GenTable>
{
    // 方法 Configure
    // 方法：Configure
    public void Configure(EntityTypeBuilder<GenTable> builder)
    {
        builder.ToTable("CodeGen_Table");
        builder.HasKey(e => e.TableName);
        builder.Property(e => e.TableName).HasMaxLength(100);
        builder.Property(e => e.ClassName).HasMaxLength(100);
        builder.Property(e => e.ModuleCode).HasMaxLength(50);
        builder.Property(e => e.FunctionName).HasMaxLength(200);
        builder.Property(e => e.FunctionAuthor).HasMaxLength(50);
        builder.Property(e => e.TableComment).HasMaxLength(500);
        builder.Property(e => e.ParentTableName).HasMaxLength(100);
        builder.Property(e => e.ParentFieldName).HasMaxLength(100);
        builder.Property(e => e.TplCategory).HasMaxLength(50);
        builder.Property(e => e.PackageName).HasMaxLength(200);
        builder.Property(e => e.BusinessName).HasMaxLength(100);
        builder.Property(e => e.TreeCode).HasMaxLength(100);
        builder.Property(e => e.TreeParentCode).HasMaxLength(100);
        builder.Property(e => e.TreeName).HasMaxLength(200);
        builder.Property(e => e.Options).HasMaxLength(2000);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.HasIndex(e => e.ModuleCode);
        builder.HasMany(e => e.Columns).WithOne().HasForeignKey(c => c.TableName).OnDelete(DeleteBehavior.Cascade);
    }
}
