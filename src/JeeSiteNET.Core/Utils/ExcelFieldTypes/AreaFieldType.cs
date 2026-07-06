using System.Collections.Concurrent;

namespace JeeSiteNET.Core.Utils.ExcelFieldTypes;

/// <summary>
/// 区域（行政区域）字段类型。与 OfficeFieldType/CompanyFieldType 设计一致。
/// 导出：区域编码（如 "AREA001"）→ "AREA001 华东区"；
/// 导入：单元格中文本 → 提取区域编码。
/// 通过静态 AreaFieldType.Register(IEnumerable{KeyValuePair{string,string}}) 注入映射表。
/// </summary>
public class AreaFieldType : IExcelFieldType
{
    /// <summary>目标属性类型：string（区域编码）。</summary>
    public Type ValueType => typeof(string);

    /// <summary>编码 → 名称 映射。使用 ConcurrentDictionary 保证并发读写线程安全。</summary>
    private static readonly ConcurrentDictionary<string, string> _codeToName = new();

    /// <summary>名称 → 编码 映射（用于支持按名称反向查找）。使用 ConcurrentDictionary 保证并发读写线程安全。</summary>
    private static readonly ConcurrentDictionary<string, string> _nameToCode = new();

    /// <summary>线程锁：保护两个映射字典的并发读写。</summary>
    private static readonly object _lock = new();

    /// <summary>
    /// 注册区域编码/名称映射，通常在应用启动时调用一次。
    /// </summary>
    /// <param name="codeNamePairs">编码-名称键值对集合。</param>
    public static void Register(IEnumerable<KeyValuePair<string, string>> codeNamePairs)
    {
        if (codeNamePairs == null) return;
        lock (_lock)
        {
            foreach (var kv in codeNamePairs)
            {
                _codeToName[kv.Key] = kv.Value;
                _nameToCode[kv.Value] = kv.Key;
            }
        }
    }

    /// <summary>清空已注册的映射（多用于单元测试）。</summary>
    public static void Clear()
    {
        lock (_lock) { _codeToName.Clear(); _nameToCode.Clear(); }
    }

    /// <summary>
    /// 导出：将对象值转换为 "编码 名称" 格式字符串。
    /// </summary>
    /// <param name="value">模型属性值（区域编码）。</param>
    /// <returns>"AREA001 华东区"，若找不到映射则仅返回编码。</returns>
    public string? ValueToCell(object? value)
    {
        if (value == null) return null;
        var code = value.ToString();
        if (string.IsNullOrWhiteSpace(code)) return null;
        lock (_lock)
        {
            if (_codeToName.TryGetValue(code, out var name))
                return $"{code} {name}";
        }
        return code;
    }

    /// <summary>
    /// 导入：将单元格文本（"AREA001 华东区" / "AREA001" / "华东区"）解析为区域编码。
    /// </summary>
    /// <param name="cellText">单元格原始文本。</param>
    /// <returns>区域编码字符串；空文本返回 null。</returns>
    public object? CellToValue(string cellText)
    {
        if (string.IsNullOrWhiteSpace(cellText)) return null;
        var trimmed = cellText.Trim();
        // 取空格前的第一段作为候选编码（兼容 "AREA001 华东区" 格式）
        var firstToken = trimmed.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? trimmed;
        lock (_lock)
        {
            // 命中编码 → 直接返回
            if (_codeToName.ContainsKey(firstToken)) return firstToken;
            // 完整名称命中 → 反向查找
            if (_nameToCode.TryGetValue(trimmed, out var codeFromName)) return codeFromName;
            // 仅第一段为名称 → 反向查找
            if (_nameToCode.TryGetValue(firstToken, out var codeFromFirstToken)) return codeFromFirstToken;
        }
        // 都未命中 → 原样返回（保留原始值让上层业务决定）
        return firstToken;
    }
}
