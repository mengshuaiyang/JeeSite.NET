using JeeSiteNET.Modules.Test.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Modules.Test.Infrastructure.EntityConfigurations;

public class TestDataConfiguration : IEntityTypeConfiguration<TestData>
{
    public void Configure(EntityTypeBuilder<TestData> builder)
    {
        builder.ToTable("Test_Data");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasMaxLength(100);
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
        builder.Property(e => e.Status).HasMaxLength(1);
        builder.Property(e => e.CreateBy).HasMaxLength(100);
        builder.Property(e => e.UpdateBy).HasMaxLength(100);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.HasMany<TestDataChild>().WithOne().HasForeignKey(e => e.TestDataId);
    }
}
