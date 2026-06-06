using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations;

public class RoleMenuConfiguration : IEntityTypeConfiguration<RoleMenu>
{
    public void Configure(EntityTypeBuilder<RoleMenu> builder)
    {
        builder.ToTable("Sys_Role_Menu");
        builder.HasKey(e => new { e.RoleCode, e.MenuCode });
        builder.Property(e => e.RoleCode).HasMaxLength(100);
        builder.Property(e => e.MenuCode).HasMaxLength(100);
        builder.HasIndex(e => e.MenuCode);
        builder.HasIndex(e => e.RoleCode);
    }
}
