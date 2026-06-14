using System.Text;

namespace JeeSiteNET.Core.Utils;

/// <summary>
/// 文本差异/补丁/匹配工具类（简化版 Diff-Match-Patch：基于最长公共子序列分段，输出 Insert/Delete/Equal 片段）
/// </summary>
public static class DiffMatchPatch
{
    /// <summary>
    /// 比较两段文本并返回差异片段列表（Diff 列表，每段标注 Insert/Delete/Equal）
    /// </summary>
    /// <param name="text1">原始文本</param>
    /// <param name="text2">新文本</param>
    /// <returns>Diff 片段列表</returns>
    public static List<Diff> DiffText(string text1, string text2)
    {
        var result = new List<Diff>();
        int lcs = LongestCommonSubsequence(text1, text2);

        // 无公共字符：直接以整段删除+插入表示
        if (lcs == 0)
        {
            if (!string.IsNullOrEmpty(text1)) result.Add(new Diff(DiffType.Delete, text1));
            if (!string.IsNullOrEmpty(text2)) result.Add(new Diff(DiffType.Insert, text2));
            return result;
        }

        // 以公共子序列为锚点分割文本，递归应用相同逻辑
        var parts = SplitByLcs(text1, text2);
        foreach (var part in parts)
            result.AddRange(part);

        return result;
    }

    /// <summary>
    /// 应用差异列表对原始文本打补丁，返回合并后的新文本
    /// </summary>
    /// <param name="originalText">原始文本</param>
    /// <param name="diffs">Diff 片段列表</param>
    /// <returns>打补丁后的新文本</returns>
    public static string PatchText(string originalText, List<Diff> diffs)
    {
        var sb = new StringBuilder(originalText);
        int offset = 0;

        // 顺序处理每段差异：Delete 在原文本中查找并移除；Insert 在当前偏移位置插入；Equal 仅移动游标
        foreach (var diff in diffs)
        {
            if (diff.Type == DiffType.Delete)
            {
                int idx = sb.ToString().IndexOf(diff.Text, offset, StringComparison.Ordinal);
                if (idx >= 0)
                {
                    sb.Remove(idx, diff.Text.Length);
                    offset = idx;
                }
            }
            else if (diff.Type == DiffType.Insert)
            {
                sb.Insert(offset, diff.Text);
                offset += diff.Text.Length;
            }
            else if (diff.Type == DiffType.Equal)
            {
                offset += diff.Text.Length;
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// 在指定文本中从起始位置匹配子串，返回匹配到的子串内容
    /// </summary>
    /// <param name="text">被搜索的文本</param>
    /// <param name="pattern">要匹配的模式字符串</param>
    /// <param name="startFrom">起始搜索索引</param>
    /// <returns>匹配到的片段；未匹配时返回空字符串</returns>
    public static string MatchPattern(string text, string pattern, int startFrom = 0)
    {
        if (startFrom >= text.Length) return "";
        int idx = text.IndexOf(pattern, startFrom, StringComparison.Ordinal);
        return idx >= 0 ? text.Substring(idx, Math.Min(pattern.Length, text.Length - idx)) : "";
    }

    /// <summary>
    /// 计算两个字符串的最长公共子序列长度（动态规划 O(n×m)）
    /// </summary>
    /// <param name="a">第一个字符串</param>
    /// <param name="b">第二个字符串</param>
    /// <returns>LCS 长度</returns>
    private static int LongestCommonSubsequence(string a, string b)
    {
        int m = a.Length, n = b.Length;
        var dp = new int[m + 1, n + 1];
        for (int i = 1; i <= m; i++)
            for (int j = 1; j <= n; j++)
                dp[i, j] = a[i - 1] == b[j - 1]
                    ? dp[i - 1, j - 1] + 1
                    : Math.Max(dp[i - 1, j], dp[i, j - 1]);
        return dp[m, n];
    }

    /// <summary>
    /// 按最长公共子序列将两段文本拆分为若干 Diff 片段组（Equal + 差异段）
    /// </summary>
    /// <param name="a">原始文本</param>
    /// <param name="b">新文本</param>
    /// <returns>分段后的 Diff 集合列表</returns>
    private static List<List<Diff>> SplitByLcs(string a, string b)
    {
        var result = new List<List<Diff>>();
        int i = 0, j = 0;

        while (i < a.Length || j < b.Length)
        {
            // 对齐公共字符：逐位匹配，相同则累积成 Equal 段
            if (i < a.Length && j < b.Length && a[i] == b[j])
            {
                var eq = new StringBuilder();
                while (i < a.Length && j < b.Length && a[i] == b[j])
                {
                    eq.Append(a[i]); i++; j++;
                }
                result.Add([new Diff(DiffType.Equal, eq.ToString())]);
            }
            else
            {
                // 否则累计 Delete（仅在 a 中出现）与 Insert（仅在 b 中出现）字符直到下一次对齐
                var del = new StringBuilder();
                var ins = new StringBuilder();

                while (i < a.Length && (j >= b.Length || a[i] != b[j]))
                {
                    del.Append(a[i]); i++;
                }
                while (j < b.Length && (i >= a.Length || a[i] != b[j]))
                {
                    ins.Append(b[j]); j++;
                }

                var segment = new List<Diff>();
                if (del.Length > 0) segment.Add(new Diff(DiffType.Delete, del.ToString()));
                if (ins.Length > 0) segment.Add(new Diff(DiffType.Insert, ins.ToString()));
                if (segment.Count > 0) result.Add(segment);
            }
        }

        return result;
    }
}

/// <summary>
/// Diff 片段类型：相同 / 插入 / 删除
/// </summary>
public enum DiffType { Equal, Insert, Delete }

/// <summary>
/// 单个差异记录（类型 + 对应文本）
/// </summary>
/// <param name="Type">差异类型</param>
/// <param name="Text">差异涉及的文本片段</param>
public record Diff(DiffType Type, string Text)
{
    /// <summary>
    /// 格式化为人类可读的单行差异表示（前缀为 " " / "+" / "-"）
    /// </summary>
    /// <returns>格式化字符串</returns>
    public override string ToString() => Type switch
    {
        DiffType.Equal => $" {Text} ",
        DiffType.Insert => $"+{Text}",
        DiffType.Delete => $"-{Text}",
        _ => Text
    };
}
