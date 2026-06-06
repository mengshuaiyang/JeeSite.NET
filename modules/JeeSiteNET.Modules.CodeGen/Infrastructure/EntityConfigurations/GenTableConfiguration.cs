using JeeSiteNET.Modules.CodeGen.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.CodeGen.Infrastructure.EntityConfigurations;

public class GenTableConfiguration : IEntityTypeConfiguration<GenTable>
{
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
        builder.HasMany(e => e.Columns).WithOne().HasForeignKey(c => c.TableName).OnDelete(DeleteBehavior.Cascade);
    }
}
