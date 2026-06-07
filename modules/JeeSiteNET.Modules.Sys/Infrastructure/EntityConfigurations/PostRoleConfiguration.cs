using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations;

public class PostRoleConfiguration : IEntityTypeConfiguration<PostRole>
{
    public void Configure(EntityTypeBuilder<PostRole> builder)
    {
        builder.ToTable("Sys_Post_Role");
        builder.HasKey(e => new { e.PostCode, e.RoleCode });
        builder.Property(e => e.PostCode).HasMaxLength(100);
        builder.Property(e => e.RoleCode).HasMaxLength(100);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.HasIndex(e => e.PostCode);
        builder.HasIndex(e => e.RoleCode);
    }
}
