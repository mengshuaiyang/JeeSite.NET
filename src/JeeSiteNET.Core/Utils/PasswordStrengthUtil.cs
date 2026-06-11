using System.Text.RegularExpressions;

namespace JeeSiteNET.Core.Utils;

public static partial class PasswordStrengthUtil
{
    [GeneratedRegex(@"[a-z]")]
    private static partial Regex LowercasePattern();
    [GeneratedRegex(@"[A-Z]")]
    private static partial Regex UppercasePattern();
    [GeneratedRegex(@"[0-9]")]
    private static partial Regex DigitPattern();
    [GeneratedRegex(@"[^a-zA-Z0-9]")]
    private static partial Regex SymbolPattern();

    public static string Evaluate(string password)
    {
        if (string.IsNullOrEmpty(password)) return "0";
        var score = 0;
        var len = password.Length;
        if (len >= 8) score++;
        if (len >= 12) score++;
        if (LowercasePattern().IsMatch(password)) score++;
        if (UppercasePattern().IsMatch(password)) score++;
        if (DigitPattern().IsMatch(password)) score++;
        if (SymbolPattern().IsMatch(password)) score++;
        return score switch
        {
            <= 2 => "0",
            3 or 4 => "1",
            5 => "2",
            _ => "3"
        };
    }
}
