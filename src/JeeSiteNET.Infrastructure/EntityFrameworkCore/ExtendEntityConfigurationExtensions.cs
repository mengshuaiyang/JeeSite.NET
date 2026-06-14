using JeeSiteNET.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JeeSiteNET.Infrastructure.EntityFrameworkCore;

/// <summary>
/// 实体配置扩展：为 ICorpEntity / IExtendEntity 提供字段长度与类型的便捷配置方法
/// </summary>
public static class ExtendEntityConfigurationExtensions
{
    /// <summary>
    /// 配置 ICorpEntity 的 CorpCode（varchar 100）和 CorpName（varchar 200）字段
    /// </summary>
    /// <typeparam name="T">实现 ICorpEntity 的实体类型</typeparam>
    /// <param name="builder">实体类型构建器</param>
    public static void ConfigureCorpFields<T>(this EntityTypeBuilder<T> builder)
        where T : class, ICorpEntity
    {
        builder.Property(e => e.CorpCode).HasMaxLength(100);
        builder.Property(e => e.CorpName).HasMaxLength(200);
    }

    /// <summary>
    /// 配置 IExtendEntity 的扩展字段（S1-S8 varchar 500；I1-I4 int；F1-F4 decimal(18,4)；D1-D4 datetime；ExtendJson）
    /// </summary>
    /// <typeparam name="T">实现 IExtendEntity 的实体类型</typeparam>
    /// <param name="builder">实体类型构建器</param>
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
