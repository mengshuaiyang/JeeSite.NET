namespace JeeSiteNET.Core.Utils.ExcelFieldTypes;

/// <summary>
/// Excel 自定义字段类型扩展接口。实现者通过 ExcelFieldAttribute.FieldType 注册到 ExcelService。
/// 两个方向：
///   ① 导出（模型 → 单元格）：ValueToCell 决定写入什么字符串
///   ② 导入（单元格 → 模型）：CellToValue 解析字符串返回对象
/// </summary>
public interface IExcelFieldType
{
    /// <summary>目标 .NET 属性类型（导出时写入的最终类型；如 string/decimal/...）。可返回 null 表示不改写。</summary>
    Type ValueType { get; }

    /// <summary>
    /// 导出：对象属性值 → Excel 单元格字符串。
    /// </summary>
    /// <param name="value">模型属性原始值。</param>
    /// <returns>要写入单元格的字符串；返回 null 表示交由 ExcelService 默认逻辑处理。</returns>
    string? ValueToCell(object? value);

    /// <summary>
    /// 导入：Excel 单元格字符串 → 对象属性值。
    /// </summary>
    /// <param name="cellText">单元格文本内容（已去除前后空白的原始文本）。</param>
    /// <returns>解析后的对象值；返回 null 表示交由默认逻辑处理。</returns>
    object? CellToValue(string cellText);
}
