using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Sys_User");
        builder.HasKey(e => e.UserCode);
        builder.Property(e => e.UserCode).HasMaxLength(100);
        builder.Property(e => e.LoginCode).HasMaxLength(100);
        builder.Property(e => e.UserName).HasMaxLength(200);
        builder.Property(e => e.Password).HasMaxLength(200);
        builder.Property(e => e.UserType).HasMaxLength(100);
        builder.Property(e => e.Avatar).HasMaxLength(500);
        builder.Property(e => e.Email).HasMaxLength(300);
        builder.Property(e => e.Phone).HasMaxLength(100);
        builder.Property(e => e.OrgCode).HasMaxLength(100);
        builder.Property(e => e.OrgName).HasMaxLength(200);
        builder.Property(e => e.LoginIp).HasMaxLength(100);
        builder.Property(e => e.LoginCount).HasColumnType("decimal(18,2)");
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.HasIndex(e => e.LoginCode).IsUnique();
        builder.HasIndex(e => e.OrgCode);
        builder.HasIndex(e => e.UserType);
    }
}
