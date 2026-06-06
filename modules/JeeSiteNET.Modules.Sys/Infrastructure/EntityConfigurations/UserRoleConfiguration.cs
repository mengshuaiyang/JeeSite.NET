using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("Sys_User_Role");
        builder.HasKey(e => new { e.UserCode, e.RoleCode });
        builder.Property(e => e.UserCode).HasMaxLength(100);
        builder.Property(e => e.RoleCode).HasMaxLength(100);
        builder.HasIndex(e => e.RoleCode);
        builder.HasIndex(e => e.UserCode);
    }
}
