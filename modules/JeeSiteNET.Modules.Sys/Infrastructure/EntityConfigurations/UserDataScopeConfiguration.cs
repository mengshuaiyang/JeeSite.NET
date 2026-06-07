using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations;

public class UserDataScopeConfiguration : IEntityTypeConfiguration<UserDataScope>
{
    public void Configure(EntityTypeBuilder<UserDataScope> builder)
    {
        builder.ToTable("Sys_User_Data_Scope");
        builder.HasKey(e => e.UserCode);
        builder.Property(e => e.UserCode).HasMaxLength(100);
        builder.Property(e => e.CtrlType).HasMaxLength(50);
        builder.Property(e => e.CtrlData).HasMaxLength(2000);
        builder.Property(e => e.CtrlPermi).HasMaxLength(10);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(1);
    }
}
