using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations;

public class RoleFieldScopeConfiguration : IEntityTypeConfiguration<RoleFieldScope>
{
    public void Configure(EntityTypeBuilder<RoleFieldScope> builder)
    {
        builder.ToTable("Sys_Role_Field_Scope");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasMaxLength(100);
        builder.Property(e => e.RoleCode).HasMaxLength(100);
        builder.Property(e => e.MenuCode).HasMaxLength(100);
        builder.Property(e => e.EntityName).HasMaxLength(200);
        builder.Property(e => e.EntityLabel).HasMaxLength(200);
        builder.Property(e => e.EntityClass).HasMaxLength(500);
        builder.Property(e => e.FieldConfig).HasColumnType("text");
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.HasIndex(e => e.RoleCode);
    }
}
