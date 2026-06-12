using System.Text;

namespace JeeSiteNET.Core.Utils.ExcelFieldTypes;

/// <summary>
/// 岗位列表字段类型（如 "POST001,POST002"）。
/// 导出：逗号分隔的岗位编码 → 编码+名称列表，每行一个
/// 导入："POST001 工程师, POST002 设计师" 或 "POST001,POST002" → 编码列表
/// </summary>
public class PostListFieldType : IExcelFieldType
{
    public Type ValueType => typeof(List<string>);

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
        IEnumerable<string>? codes = null;
        if (value is IEnumerable<string> en) codes = en;
        else if (value is string s) codes = s.Split(',', StringSplitOptions.RemoveEmptyEntries);

        if (codes == null || !codes.Any()) return null;

        var sb = new StringBuilder();
        lock (_lock)
        {
            foreach (var code in codes)
            {
                var c = code.Trim();
                if (string.IsNullOrEmpty(c)) continue;
                if (sb.Length > 0) sb.Append(", ");
                if (_codeToName.TryGetValue(c, out var name))
                    sb.Append($"{c} {name}");
                else
                    sb.Append(c);
            }
        }
        return sb.ToString();
    }

    public object? CellToValue(string cellText)
    {
        if (string.IsNullOrWhiteSpace(cellText)) return null;
        var parts = cellText.Split(',', StringSplitOptions.RemoveEmptyEntries);
        var result = new List<string>();
        lock (_lock)
        {
            foreach (var part in parts)
            {
                var trimmed = part.Trim();
                var firstToken = trimmed.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? trimmed;
                if (_codeToName.ContainsKey(firstToken))
                    result.Add(firstToken);
                else if (_nameToCode.TryGetValue(trimmed, out var codeFromName))
                    result.Add(codeFromName);
                else
                    result.Add(firstToken);
            }
        }
        return result;
    }
}
