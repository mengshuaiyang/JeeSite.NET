using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations;

public class RoleDataScopeConfiguration : IEntityTypeConfiguration<RoleDataScope>
{
    public void Configure(EntityTypeBuilder<RoleDataScope> builder)
    {
        builder.ToTable("Sys_Role_Data_Scope");
        builder.HasKey(e => new { e.RoleCode, e.MenuCode });
        builder.Property(e => e.RoleCode).HasMaxLength(100);
        builder.Property(e => e.MenuCode).HasMaxLength(100);
        builder.Property(e => e.RuleName).HasMaxLength(200);
        builder.Property(e => e.RuleType).HasMaxLength(50);
        builder.Property(e => e.RuleConfig).HasColumnType("text");
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.HasIndex(e => e.RoleCode);
    }
}
