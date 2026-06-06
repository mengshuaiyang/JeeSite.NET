using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Sys_Role");
        builder.HasKey(e => e.RoleCode);
        builder.Property(e => e.RoleCode).HasMaxLength(100);
        builder.Property(e => e.RoleName).HasMaxLength(200);
        builder.Property(e => e.RoleType).HasMaxLength(100);
        builder.Property(e => e.IsSys).HasMaxLength(1);
        builder.Property(e => e.UserType).HasMaxLength(100);
        builder.Property(e => e.Sort).HasColumnType("decimal(10,2)");
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(1);
    }
}
