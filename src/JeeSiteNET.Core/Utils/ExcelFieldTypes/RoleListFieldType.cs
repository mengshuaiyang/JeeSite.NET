using System.Text;

namespace JeeSiteNET.Core.Utils.ExcelFieldTypes;

/// <summary>
/// 角色列表字段类型。与 PostListFieldType 设计一致，用于角色编码 ↔ 名称映射。
/// 导出：角色编码列表 → "编码 名称, 编码 名称, ..."；
/// 导入：逗号分隔文本 → 编码列表 List{string}。
/// 通过静态 RoleListFieldType.Register(...) 注入角色编码/名称映射表。
/// </summary>
public class RoleListFieldType : IExcelFieldType
{
    /// <summary>目标属性类型：List{string}（角色编码列表）。</summary>
    public Type ValueType => typeof(List<string>);

    /// <summary>编码 → 名称 映射。</summary>
    private static readonly Dictionary<string, string> _codeToName = new();

    /// <summary>名称 → 编码 映射。</summary>
    private static readonly Dictionary<string, string> _nameToCode = new();

    /// <summary>线程锁。</summary>
    private static readonly object _lock = new();

    /// <summary>注册角色编码-名称映射。</summary>
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
    /// 导出：List{string} 或逗号分隔字符串 → "编码 名称, ..."。
    /// </summary>
    /// <param name="value">List{string} 或 string（逗号分隔编码）。</param>
    /// <returns>格式化列表字符串。</returns>
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

    /// <summary>
    /// 导入：逗号分隔的角色字符串 → List{string}（编码列表）。
    /// </summary>
    /// <param name="cellText">角色文本（含编码或编码+名称）。</param>
    /// <returns>角色编码列表；空文本返回 null。</returns>
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
