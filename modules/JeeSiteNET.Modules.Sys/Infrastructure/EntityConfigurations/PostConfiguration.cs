    // 引入 JeeSiteNET.Infrastructure.EntityFrameworkCore 命名空间
// 引入命名空间：JeeSiteNET.Infrastructure.EntityFrameworkCore
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
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

// 定义class PostConfiguration
// 定义类：PostConfiguration
public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    // 方法 Configure
    // 方法：Configure
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Sys_Post");
        builder.HasKey(e => e.PostCode);
        builder.Property(e => e.PostCode).HasMaxLength(100);
        builder.Property(e => e.PostName).HasMaxLength(200);
        builder.Property(e => e.ViewCode).HasMaxLength(100);
        builder.Property(e => e.PostType).HasMaxLength(100);
        builder.Property(e => e.PostSort).HasColumnType("decimal(10,2)");
        builder.Property(e => e.OrgCode).HasMaxLength(100);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.ConfigureCorpFields();
        builder.HasIndex(e => e.OrgCode);
        builder.HasIndex(e => e.PostSort);
    }
}
