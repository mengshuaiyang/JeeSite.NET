using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JeeSiteNET.Modules.Sys.Domain.Entities;

namespace JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations;

public class EmpUserConfiguration : IEntityTypeConfiguration<EmpUser>
{
    public void Configure(EntityTypeBuilder<EmpUser> builder)
    {
        builder.ToTable("Sys_EmpUser");
        builder.HasKey(e => new { e.EmpCode, e.UserCode });
        builder.Property(e => e.EmpCode).HasMaxLength(100).IsRequired();
        builder.Property(e => e.UserCode).HasMaxLength(100).IsRequired();
        builder.Property(e => e.EmpName).HasMaxLength(200);
        builder.Property(e => e.LoginCode).HasMaxLength(100);
        builder.Property(e => e.UserName).HasMaxLength(200);
        builder.HasIndex(e => e.EmpCode);
        builder.HasIndex(e => e.UserCode).IsUnique();
    }
}
