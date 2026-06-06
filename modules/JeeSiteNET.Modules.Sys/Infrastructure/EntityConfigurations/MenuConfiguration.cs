using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations;

public class MenuConfiguration : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.ToTable("Sys_Menu");
        builder.HasKey(e => e.MenuCode);
        builder.Property(e => e.MenuCode).HasMaxLength(100);
        builder.Property(e => e.MenuName).HasMaxLength(200);
        builder.Property(e => e.MenuHref).HasMaxLength(1000);
        builder.Property(e => e.MenuTarget).HasMaxLength(20);
        builder.Property(e => e.MenuIcon).HasMaxLength(100);
        builder.Property(e => e.Permission).HasMaxLength(500);
        builder.Property(e => e.IsShow).HasMaxLength(1);
        builder.Property(e => e.ModuleCode).HasMaxLength(100);
        builder.Property(e => e.Weight).HasColumnType("decimal(10,2)");
        builder.Property(e => e.TreeSort).HasColumnType("decimal(10,2)");
        builder.Property(e => e.TreeLevel).HasColumnType("decimal(10,2)");
        builder.Property(e => e.ParentCode).HasMaxLength(100);
        builder.Property(e => e.ParentCodes).HasMaxLength(2000);
        builder.Property(e => e.TreeSorts).HasMaxLength(2000);
        builder.Property(e => e.TreeLeaf).HasMaxLength(1);
        builder.Property(e => e.TreeNames).HasMaxLength(2000);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.HasIndex(e => e.ParentCode);
    }
}
