using System.Text.RegularExpressions;

namespace JeeSiteNET.Core.Utils;

/// <summary>
/// 密码强度评估工具：基于长度 + 字符类型多样性（大写/小写/数字/符号）打分，
/// 返回 0-3 级的字符串等级，用于前端提示用户改善密码复杂度。
/// 不处理真实密码存储，真正的密码哈希应使用 PBKDF2 / bcrypt / argon2 等算法。
/// </summary>
public static partial class PasswordStrengthUtil
{
    /// <summary>
    /// 匹配小写英文字母的正则表达式（编译为 Regex 源码生成器，性能更佳）。
    /// </summary>
    /// <returns>已编译的小写字母匹配器。</returns>
    [GeneratedRegex(@"[a-z]")]
    private static partial Regex LowercasePattern();

    /// <summary>
    /// 匹配大写英文字母的正则表达式。
    /// </summary>
    [GeneratedRegex(@"[A-Z]")]
    private static partial Regex UppercasePattern();

    /// <summary>
    /// 匹配数字字符的正则表达式。
    /// </summary>
    [GeneratedRegex(@"[0-9]")]
    private static partial Regex DigitPattern();

    /// <summary>
    /// 匹配非字母数字字符（符号）的正则表达式。
    /// </summary>
    [GeneratedRegex(@"[^a-zA-Z0-9]")]
    private static partial Regex SymbolPattern();

    /// <summary>
    /// 评估密码强度，返回等级字符串：
    /// <list type="bullet">
    /// <item><term>0</term><description>极弱：空或字符多样性不足。</description></item>
    /// <item><term>1</term><description>弱：长度 &lt; 8，或长度 8-11 且字符类型 &lt; 4。</description></item>
    /// <item><term>2</term><description>中：长度 &gt;= 8 并包含全部 4 种字符类型。</description></item>
    /// <item><term>3</term><description>强：长度 &gt;= 12 并包含全部 4 种字符类型。</description></item>
    /// </list>
    /// </summary>
    /// <param name="password">待评估的密码原文。</param>
    /// <returns>等级字符串（"0" / "1" / "2" / "3"）。</returns>
    public static string Evaluate(string password)
    {
        if (string.IsNullOrEmpty(password)) return "0";

        // 统计 4 种字符类型的命中数（0-4 分）
        var typeCount = 0;
        if (LowercasePattern().IsMatch(password)) typeCount++;
        if (UppercasePattern().IsMatch(password)) typeCount++;
        if (DigitPattern().IsMatch(password)) typeCount++;
        if (SymbolPattern().IsMatch(password)) typeCount++;

        // 长度越长 + 字符越多样 = 等级越高
        // 长度 12 且 4 种字符齐全 → 强
        if (password.Length >= 12 && typeCount == 4) return "3";
        // 长度 8 且 4 种字符齐全 → 中
        if (password.Length >= 8 && typeCount == 4) return "2";
        // 长度 &lt; 8 → 弱（基础长度不达标）
        if (password.Length < 8) return "0";
        // 长度达标但字符类型不全 → 弱
        return "1";
    }
}
