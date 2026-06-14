namespace JeeSiteNET.Core.Utils.ExcelFieldTypes;

/// <summary>
/// 公司字段类型。与 OfficeFieldType/AreaFieldType 设计一致，区分公司/机构/区域三个业务域。
/// 导出：公司编码（如 "C0001"）→ "C0001 总公司"；
/// 导入：文本 → 公司编码。
/// 通过静态 CompanyFieldType.Register(...) 注入映射表。
/// </summary>
public class CompanyFieldType : IExcelFieldType
{
    /// <summary>目标属性类型：string（公司编码）。</summary>
    public Type ValueType => typeof(string);

    /// <summary>编码 → 名称 映射。</summary>
    private static readonly Dictionary<string, string> _codeToName = new();

    /// <summary>名称 → 编码 映射。</summary>
    private static readonly Dictionary<string, string> _nameToCode = new();

    /// <summary>线程锁。</summary>
    private static readonly object _lock = new();

    /// <summary>
    /// 注册公司编码/名称映射。
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

    /// <summary>清空映射。</summary>
    public static void Clear()
    {
        lock (_lock) { _codeToName.Clear(); _nameToCode.Clear(); }
    }

    /// <summary>
    /// 导出：对象值 → "编码 名称"。
    /// </summary>
    /// <param name="value">公司编码。</param>
    /// <returns>"C0001 总公司" 或仅编码。</returns>
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
    /// 导入：单元格文本 → 公司编码。
    /// </summary>
    /// <param name="cellText">单元格文本。</param>
    /// <returns>公司编码。</returns>
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
