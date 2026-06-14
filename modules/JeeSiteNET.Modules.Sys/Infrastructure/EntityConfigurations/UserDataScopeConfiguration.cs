    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;
    // 引入 Microsoft.EntityFrameworkCore 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore
using Microsoft.EntityFrameworkCore;
    // 引入 Microsoft.EntityFrameworkCore.Metadata.Builders 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore.Metadata.Builders
using Microsoft.EntityFrameworkCore.Metadata.Builders;

// 定义 JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations
namespace JeeSiteNET.Modules.Sys.Infrastructure.EntityConfigurations;

// 定义class UserDataScopeConfiguration
// 定义类：UserDataScopeConfiguration
public class UserDataScopeConfiguration : IEntityTypeConfiguration<UserDataScope>
{
    // 方法 Configure
    // 方法：Configure
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
