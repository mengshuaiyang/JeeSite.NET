using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations;

public class CompanyOfficeConfiguration : IEntityTypeConfiguration<CompanyOffice>
{
    public void Configure(EntityTypeBuilder<CompanyOffice> builder)
    {
        builder.ToTable("Sys_Company_Office");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasMaxLength(100);
        builder.Property(e => e.CompanyCode).HasMaxLength(100);
        builder.Property(e => e.OfficeCode).HasMaxLength(100);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.HasIndex(e => e.CompanyCode);
        builder.HasIndex(e => e.OfficeCode);
    }
}
