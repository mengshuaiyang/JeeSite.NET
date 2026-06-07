using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations;

public class EmployeePostConfiguration : IEntityTypeConfiguration<EmployeePost>
{
    public void Configure(EntityTypeBuilder<EmployeePost> builder)
    {
        builder.ToTable("Sys_Employee_Post");
        builder.HasKey(e => new { e.EmpCode, e.PostCode });
        builder.Property(e => e.EmpCode).HasMaxLength(100);
        builder.Property(e => e.PostCode).HasMaxLength(100);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.HasIndex(e => e.EmpCode);
        builder.HasIndex(e => e.PostCode);
    }
}
