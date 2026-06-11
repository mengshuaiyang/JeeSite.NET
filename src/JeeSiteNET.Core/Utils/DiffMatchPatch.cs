using System.Text;

namespace JeeSiteNET.Core.Utils;

public static class DiffMatchPatch
{
    public static List<Diff> DiffText(string text1, string text2)
    {
        var result = new List<Diff>();
        int lcs = LongestCommonSubsequence(text1, text2);

        if (lcs == 0)
        {
            if (!string.IsNullOrEmpty(text1)) result.Add(new Diff(DiffType.Delete, text1));
            if (!string.IsNullOrEmpty(text2)) result.Add(new Diff(DiffType.Insert, text2));
            return result;
        }

        var parts = SplitByLcs(text1, text2);
        foreach (var part in parts)
            result.AddRange(part);

        return result;
    }

    public static string PatchText(string originalText, List<Diff> diffs)
    {
        var sb = new StringBuilder(originalText);
        int offset = 0;

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

    public static string MatchPattern(string text, string pattern, int startFrom = 0)
    {
        if (startFrom >= text.Length) return "";
        int idx = text.IndexOf(pattern, startFrom, StringComparison.Ordinal);
        return idx >= 0 ? text.Substring(idx, Math.Min(pattern.Length, text.Length - idx)) : "";
    }

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

    private static List<List<Diff>> SplitByLcs(string a, string b)
    {
        var result = new List<List<Diff>>();
        int i = 0, j = 0;

        while (i < a.Length || j < b.Length)
        {
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

public enum DiffType { Equal, Insert, Delete }

public record Diff(DiffType Type, string Text)
{
    public override string ToString() => Type switch
    {
        DiffType.Equal => $" {Text} ",
        DiffType.Insert => $"+{Text}",
        DiffType.Delete => $"-{Text}",
        _ => Text
    };
}
