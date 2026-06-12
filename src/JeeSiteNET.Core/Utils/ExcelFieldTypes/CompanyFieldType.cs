namespace JeeSiteNET.Core.Utils.ExcelFieldTypes;

/// <summary>公司字段类型。与 OfficeFieldType 设计一致，区分公司/机构两个业务域。</summary>
public class CompanyFieldType : IExcelFieldType
{
    public Type ValueType => typeof(string);

    private static readonly Dictionary<string, string> _codeToName = new();
    private static readonly Dictionary<string, string> _nameToCode = new();
    private static readonly object _lock = new();

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
