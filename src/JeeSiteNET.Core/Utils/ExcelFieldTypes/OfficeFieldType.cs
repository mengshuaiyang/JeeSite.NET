namespace JeeSiteNET.Core.Utils.ExcelFieldTypes;

/// <summary>
/// 机构字段类型。
///
/// 导出：机构编码（如 "OFF001"）→ 机构编码 + 名称（如 "OFF001 人力资源部"）
/// 导入：Excel 单元格中含 "OFF001 人力资源部" 或 "OFF001" → 提取机构编码写入模型
///
/// 通过静态 OfficeFieldType.Register(Dictionary&lt;string, string&gt;) 注入映射表。
/// </summary>
public class OfficeFieldType : IExcelFieldType
{
    public Type ValueType => typeof(string);

    private static readonly Dictionary<string, string> _codeToName = new();
    private static readonly Dictionary<string, string> _nameToCode = new();
    private static readonly object _lock = new();

    /// <summary>注册映射表（code → name）。通常在应用启动时调用。</summary>
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

    public static void Clear()
    {
        lock (_lock) { _codeToName.Clear(); _nameToCode.Clear(); }
    }

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

    public object? CellToValue(string cellText)
    {
        if (string.IsNullOrWhiteSpace(cellText)) return null;
        var trimmed = cellText.Trim();

        var firstToken = trimmed.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? trimmed;
        lock (_lock)
        {
            if (_codeToName.ContainsKey(firstToken)) return firstToken;
            if (_nameToCode.TryGetValue(trimmed, out var codeFromName)) return codeFromName;
            if (_nameToCode.TryGetValue(firstToken, out var codeFromFirstToken)) return codeFromFirstToken;
        }
        return firstToken;
    }
}
