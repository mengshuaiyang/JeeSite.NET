using System.Text;

namespace JeeSiteNET.Core.Utils.ExcelFieldTypes;

/// <summary>
/// 岗位列表字段类型（如 "POST001,POST002"）。
/// 导出：逗号分隔的岗位编码 → "POST001 工程师, POST002 设计师"；
/// 导入：包含编码或编码+名称的逗号分隔文本 → 编码列表。
/// 通过静态 PostListFieldType.Register 注入岗位编码/名称映射表。
/// </summary>
public class PostListFieldType : IExcelFieldType
{
    /// <summary>
    /// 目标属性类型：字符串列表（岗位编码列表）
    /// </summary>
    public Type ValueType => typeof(List<string>);

    /// <summary>
    /// 岗位编码 → 岗位名称 的映射表
    /// </summary>
    private static readonly Dictionary<string, string> _codeToName = new();

    /// <summary>
    /// 岗位名称 → 岗位编码 的反向映射表
    /// </summary>
    private static readonly Dictionary<string, string> _nameToCode = new();

    /// <summary>
    /// 用于并发读写静态映射表的锁对象
    /// </summary>
    private static readonly object _lock = new();

    /// <summary>
    /// 注册岗位编码-名称映射（支持后续导入/导出时按名称或编码双向解析）
    /// </summary>
    /// <param name="codeNamePairs">编码-名称键值对集合</param>
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

    /// <summary>
    /// 清空已注册的编码-名称映射
    /// </summary>
    public static void Clear()
    {
        lock (_lock) { _codeToName.Clear(); _nameToCode.Clear(); }
    }

    /// <summary>
    /// 导出：编码列表或逗号分隔字符串 → "编码 名称, 编码 名称, ..." 格式的可读文本
    /// </summary>
    /// <param name="value">字符串列表（IEnumerable string）或逗号分隔编码字符串；null 返回 null</param>
    /// <returns>格式化后的岗位列表字符串；空列表返回 null</returns>
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
    /// 导入：逗号分隔的岗位字符串 → 编码列表；优先识别已注册编码，其次按名称反查，再次保留原始输入
    /// </summary>
    /// <param name="cellText">单元格文本，如 "POST001 工程师, POST002 设计师" 或 "POST001,POST002"</param>
    /// <returns>岗位编码列表；空文本返回 null</returns>
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
