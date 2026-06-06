using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations;

public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.ToTable("Sys_Organization");
        builder.HasKey(e => e.OrgCode);
        builder.Property(e => e.OrgCode).HasMaxLength(100);
        builder.Property(e => e.OrgName).HasMaxLength(200);
        builder.Property(e => e.OrgType).HasMaxLength(100);
        builder.Property(e => e.OrgTypeName).HasMaxLength(200);
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
        builder.HasIndex(e => e.ParentCodes);
        builder.HasIndex(e => e.TreeSorts);
    }
}
