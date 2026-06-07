using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations;

public class LangConfiguration : IEntityTypeConfiguration<Lang>
{
    public void Configure(EntityTypeBuilder<Lang> builder)
    {
        builder.ToTable("Sys_Lang");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.ModuleCode).HasMaxLength(100);
        builder.Property(e => e.LangCode).HasMaxLength(500);
        builder.Property(e => e.LangText).HasMaxLength(500);
        builder.Property(e => e.LangType).HasMaxLength(50);
        builder.HasIndex(e => new { e.LangCode, e.LangType }).IsUnique();
    }
}
