    // 引入 JeeSiteNET.Modules.Test.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Test.Domain.Entities
using JeeSiteNET.Modules.Test.Domain.Entities;
    // 引入 Microsoft.EntityFrameworkCore 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore
using Microsoft.EntityFrameworkCore;
    // 引入 Microsoft.EntityFrameworkCore.Metadata.Builders 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore.Metadata.Builders
using Microsoft.EntityFrameworkCore.Metadata.Builders;

// 定义 JeeSiteNET.Modules.Test.Infrastructure.EntityConfigurations 命名空间
// 定义命名空间：JeeSiteNET.Modules.Test.Infrastructure.EntityConfigurations
namespace JeeSiteNET.Modules.Test.Infrastructure.EntityConfigurations;

// 定义class TestDataChildConfiguration
// 定义类：TestDataChildConfiguration
public class TestDataChildConfiguration : IEntityTypeConfiguration<TestDataChild>
{
    // 方法 Configure
    // 方法：Configure
    public void Configure(EntityTypeBuilder<TestDataChild> builder)
    {
        builder.ToTable("Test_Data_Child");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasMaxLength(100);
        builder.Property(e => e.TestDataId).HasMaxLength(100);
        builder.Property(e => e.TestInput).HasMaxLength(200);
        builder.Property(e => e.TestTextarea).HasMaxLength(500);
        builder.Property(e => e.TestSelect).HasMaxLength(10);
        builder.Property(e => e.TestSelectMultiple).HasMaxLength(200);
        builder.Property(e => e.TestRadio).HasMaxLength(10);
        builder.Property(e => e.TestCheckbox).HasMaxLength(200);
        builder.Property(e => e.TestUserCode).HasMaxLength(100);
        builder.Property(e => e.TestOfficeCode).HasMaxLength(100);
        builder.Property(e => e.TestAreaCode).HasMaxLength(100);
        builder.Property(e => e.TestAreaName).HasMaxLength(200);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
    }
}
