using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Sys_User");
        builder.HasKey(e => e.UserCode);
        builder.Property(e => e.UserCode).HasMaxLength(100);
        builder.Property(e => e.LoginCode).HasMaxLength(100);
        builder.Property(e => e.UserName).HasMaxLength(200);
        builder.Property(e => e.Password).HasMaxLength(200);
        builder.Property(e => e.UserType).HasMaxLength(100);
        builder.Property(e => e.Avatar).HasMaxLength(500);
        builder.Property(e => e.Email).HasMaxLength(300);
        builder.Property(e => e.Phone).HasMaxLength(100);
        builder.Property(e => e.OrgCode).HasMaxLength(100);
        builder.Property(e => e.OrgName).HasMaxLength(200);
        builder.Property(e => e.Sex).HasMaxLength(1);
        builder.Property(e => e.Sign).HasMaxLength(500);
        builder.Property(e => e.WxOpenid).HasMaxLength(100);
        builder.Property(e => e.MobileImei).HasMaxLength(100);
        builder.Property(e => e.RefCode).HasMaxLength(100);
        builder.Property(e => e.RefName).HasMaxLength(200);
        builder.Property(e => e.MgrType).HasMaxLength(50);
        builder.Property(e => e.PwdSecurityLevel).HasMaxLength(1);
        builder.Property(e => e.LoginIp).HasMaxLength(100);
        builder.Property(e => e.LoginCount).HasColumnType("decimal(18,2)");
        builder.Property(e => e.PwdUpdateRecord).HasMaxLength(2000);
        builder.Property(e => e.PwdQuestion).HasMaxLength(200);
        builder.Property(e => e.PwdQuestionAnswer).HasMaxLength(200);
        builder.Property(e => e.FreezeCause).HasMaxLength(500);
        builder.Property(e => e.UserWeight).HasColumnType("decimal(10,2)");
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.ConfigureCorpFields();
        builder.ConfigureExtendFields();
        builder.HasIndex(e => e.LoginCode).IsUnique();
        builder.HasIndex(e => e.OrgCode);
        builder.HasIndex(e => e.UserType);
    }
}
