using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Sys_Employee");
        builder.HasKey(e => e.EmpCode);
        builder.Property(e => e.EmpCode).HasMaxLength(100);
        builder.Property(e => e.EmpNo).HasMaxLength(100);
        builder.Property(e => e.EmpName).HasMaxLength(100);
        builder.Property(e => e.EmpNameEn).HasMaxLength(100);
        builder.Property(e => e.OfficeCode).HasMaxLength(100);
        builder.Property(e => e.OfficeName).HasMaxLength(200);
        builder.Property(e => e.CompanyCode).HasMaxLength(100);
        builder.Property(e => e.CompanyName).HasMaxLength(200);
        builder.Property(e => e.CorpCode).HasMaxLength(100);
        builder.Property(e => e.CorpName).HasMaxLength(200);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.Property(e => e.ExtendJson).HasColumnType("text");
        builder.HasIndex(e => e.EmpNo);
        builder.HasIndex(e => e.OfficeCode);
        builder.HasIndex(e => e.CompanyCode);
    }
}
