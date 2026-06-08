using System.Text.RegularExpressions;

namespace JeeSiteNET.Core.Utils;

public static partial class IdcardUtil
{
    [GeneratedRegex(@"^[1-9]\d{5}(18|19|20)?\d{2}(0[1-9]|1[0-2])(0[1-9]|[12]\d|3[01])\d{3}[\dXx]?$")]
    private static partial Regex IdcardPattern();

    [GeneratedRegex(@"^1[3-9]\d{9}$")]
    private static partial Regex MobilePattern();

    [GeneratedRegex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
    private static partial Regex EmailPattern();

    public static bool ValidateIdcard(string idcard)
    {
        if (!IdcardPattern().IsMatch(idcard)) return false;
        if (idcard.Length == 18)
        {
            var factors = new[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
            var parity = "10X98765432";
            var sum = 0;
            for (int i = 0; i < 17; i++)
                sum += (idcard[i] - '0') * factors[i];
            return parity[sum % 11] == char.ToUpper(idcard[17]);
        }
        return true;
    }

    public static bool ValidateMobile(string mobile) => MobilePattern().IsMatch(mobile);
    public static bool ValidateEmail(string email) => EmailPattern().IsMatch(email);
}
