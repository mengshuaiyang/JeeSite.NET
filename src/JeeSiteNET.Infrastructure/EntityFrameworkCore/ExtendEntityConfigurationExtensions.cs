using JeeSiteNET.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Infrastructure.EntityFrameworkCore;

public static class ExtendEntityConfigurationExtensions
{
    public static void ConfigureCorpFields<T>(this EntityTypeBuilder<T> builder)
        where T : class, ICorpEntity
    {
        builder.Property(e => e.CorpCode).HasMaxLength(100);
        builder.Property(e => e.CorpName).HasMaxLength(200);
    }

    public static void ConfigureExtendFields<T>(this EntityTypeBuilder<T> builder)
        where T : class, IExtendEntity
    {
        builder.Property(e => e.ExtendS1).HasMaxLength(500);
        builder.Property(e => e.ExtendS2).HasMaxLength(500);
        builder.Property(e => e.ExtendS3).HasMaxLength(500);
        builder.Property(e => e.ExtendS4).HasMaxLength(500);
        builder.Property(e => e.ExtendS5).HasMaxLength(500);
        builder.Property(e => e.ExtendS6).HasMaxLength(500);
        builder.Property(e => e.ExtendS7).HasMaxLength(500);
        builder.Property(e => e.ExtendS8).HasMaxLength(500);
        builder.Property(e => e.ExtendI1);
        builder.Property(e => e.ExtendI2);
        builder.Property(e => e.ExtendI3);
        builder.Property(e => e.ExtendI4);
        builder.Property(e => e.ExtendF1).HasColumnType("decimal(18,4)");
        builder.Property(e => e.ExtendF2).HasColumnType("decimal(18,4)");
        builder.Property(e => e.ExtendF3).HasColumnType("decimal(18,4)");
        builder.Property(e => e.ExtendF4).HasColumnType("decimal(18,4)");
        builder.Property(e => e.ExtendD1);
        builder.Property(e => e.ExtendD2);
        builder.Property(e => e.ExtendD3);
        builder.Property(e => e.ExtendD4);
        builder.Property(e => e.ExtendJson);
    }
}
