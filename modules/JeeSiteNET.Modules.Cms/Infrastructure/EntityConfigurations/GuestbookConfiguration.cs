    // 引入 JeeSiteNET.Infrastructure.EntityFrameworkCore 命名空间
// 引入命名空间：JeeSiteNET.Infrastructure.EntityFrameworkCore
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
    // 引入 JeeSiteNET.Modules.Cms.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Cms.Domain.Entities
using JeeSiteNET.Modules.Cms.Domain.Entities;
    // 引入 Microsoft.EntityFrameworkCore 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore
using Microsoft.EntityFrameworkCore;
    // 引入 Microsoft.EntityFrameworkCore.Metadata.Builders 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore.Metadata.Builders
using Microsoft.EntityFrameworkCore.Metadata.Builders;

// 定义 JeeSiteNET.Modules.Cms.Infrastructure.EntityConfigurations 命名空间
// 定义命名空间：JeeSiteNET.Modules.Cms.Infrastructure.EntityConfigurations
namespace JeeSiteNET.Modules.Cms.Infrastructure.EntityConfigurations;

// 定义class GuestbookConfiguration
// 定义类：GuestbookConfiguration
public class GuestbookConfiguration : IEntityTypeConfiguration<Guestbook>
{
    // 方法 Configure
    // 方法：Configure
    public void Configure(EntityTypeBuilder<Guestbook> builder)
    {
        builder.ToTable("Cms_Guestbook");
        builder.HasKey(e => e.GbCode);
        builder.Property(e => e.GbCode).HasMaxLength(100);
        builder.Property(e => e.GbType).HasMaxLength(1);
        builder.Property(e => e.Content).HasMaxLength(500);
        builder.Property(e => e.Name).HasMaxLength(100);
        builder.Property(e => e.Email).HasMaxLength(100);
        builder.Property(e => e.Phone).HasMaxLength(100);
        builder.Property(e => e.WorkUnit).HasMaxLength(100);
        builder.Property(e => e.Ip).HasMaxLength(100);
        builder.Property(e => e.ReUserCode).HasMaxLength(100);
        builder.Property(e => e.ReContent).HasMaxLength(500);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.ConfigureCorpFields();
        builder.HasIndex(e => e.GbType);
    }
}
