using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations;

public class ModuleConfiguration : IEntityTypeConfiguration<Module>
{
    public void Configure(EntityTypeBuilder<Module> builder)
    {
        builder.ToTable("Sys_Module");
        builder.HasKey(e => e.ModuleCode);
        builder.Property(e => e.ModuleCode).HasMaxLength(100);
        builder.Property(e => e.ModuleName).HasMaxLength(200);
        builder.Property(e => e.ModuleVersion).HasMaxLength(50);
        builder.Property(e => e.MainClass).HasMaxLength(500);
        builder.Property(e => e.IsEnabled).HasMaxLength(1);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.HasIndex(e => e.ModuleName);
    }
}
