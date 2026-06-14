    // 引入 JeeSiteNET.Modules.Test.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Test.Domain.Entities
using JeeSiteNET.Modules.Test.Domain.Entities;
    // 引入 JeeSiteNET.Infrastructure.EntityFrameworkCore 命名空间
// 引入命名空间：JeeSiteNET.Infrastructure.EntityFrameworkCore
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
    // 引入 Microsoft.EntityFrameworkCore 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore
using Microsoft.EntityFrameworkCore;
    // 引入 Microsoft.EntityFrameworkCore.Metadata.Builders 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore.Metadata.Builders
using Microsoft.EntityFrameworkCore.Metadata.Builders;

// 定义 JeeSiteNET.Modules.Test.Infrastructure.EntityConfigurations 命名空间
// 定义命名空间：JeeSiteNET.Modules.Test.Infrastructure.EntityConfigurations
namespace JeeSiteNET.Modules.Test.Infrastructure.EntityConfigurations;

// 定义class TestTreeConfiguration
// 定义类：TestTreeConfiguration
public class TestTreeConfiguration : IEntityTypeConfiguration<TestTree>
{
    // 方法 Configure
    // 方法：Configure
    public void Configure(EntityTypeBuilder<TestTree> builder)
    {
        builder.ToTable("Test_Tree");
        builder.HasKey(e => e.TreeCode);
        builder.Property(e => e.TreeCode).HasMaxLength(100);
        builder.Property(e => e.TreeName).HasMaxLength(200);
        builder.Property(e => e.ParentCode).HasMaxLength(100);
        builder.Property(e => e.ParentCodes).HasMaxLength(767);
        builder.Property(e => e.TreeSorts).HasMaxLength(767);
        builder.Property(e => e.TreeNames).HasMaxLength(767);
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.HasIndex(e => e.ParentCode);
        builder.HasIndex(e => e.TreeSort);
    }
}
