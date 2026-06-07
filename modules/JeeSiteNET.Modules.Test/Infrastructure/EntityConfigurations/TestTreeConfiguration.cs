using JeeSiteNET.Modules.Test.Domain.Entities;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Test.Infrastructure.EntityConfigurations;

public class TestTreeConfiguration : IEntityTypeConfiguration<TestTree>
{
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
