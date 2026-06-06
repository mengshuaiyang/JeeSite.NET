using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Sys_Post");
        builder.HasKey(e => e.PostCode);
        builder.Property(e => e.PostCode).HasMaxLength(100);
        builder.Property(e => e.PostName).HasMaxLength(200);
        builder.Property(e => e.OrgCode).HasMaxLength(100);
        builder.Property(e => e.Sort).HasColumnType("decimal(10,2)");
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.HasIndex(e => e.OrgCode);
        builder.HasIndex(e => e.Sort);
    }
}
