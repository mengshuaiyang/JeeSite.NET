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

// 定义class BizCategoryConfiguration
// 定义类：BizCategoryConfiguration
public class BizCategoryConfiguration : IEntityTypeConfiguration<BizCategory>
{
    // 方法 Configure
    // 方法：Configure
    public void Configure(EntityTypeBuilder<BizCategory> builder)
    {
        builder.ToTable("Biz_Category");
        builder.HasKey(e => e.CategoryCode);
        builder.Property(e => e.CategoryCode).HasMaxLength(100);
        builder.Property(e => e.ViewCode).HasMaxLength(500);
        builder.Property(e => e.CategoryName).HasMaxLength(100);
        builder.Property(e => e.ParentCode).HasMaxLength(100);
        builder.Property(e => e.ParentCodes).HasMaxLength(767);
        builder.Property(e => e.TreeSorts).HasMaxLength(767);
        builder.Property(e => e.TreeNames).HasMaxLength(767);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.ConfigureCorpFields();
        builder.HasIndex(e => e.ViewCode);
        builder.HasIndex(e => e.ParentCode);
        builder.HasIndex(e => e.TreeSort);
    }
}
