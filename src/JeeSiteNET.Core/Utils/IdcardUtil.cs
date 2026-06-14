using System.Text.RegularExpressions;

namespace JeeSiteNET.Core.Utils;

/// <summary>
/// 身份证/手机号/邮箱 通用校验工具类
/// </summary>
public static partial class IdcardUtil
{
    /// <summary>
    /// 18 位/15 位身份证号码正则（行政区划 + 出生年月日 + 顺序号 + 校验位）
    /// </summary>
    /// <returns>编译生成的 Regex 实例</returns>
    [GeneratedRegex(@"^[1-9]\d{5}(18|19|20)?\d{2}(0[1-9]|1[0-2])(0[1-9]|[12]\d|3[01])\d{3}[\dXx]?$")]
    private static partial Regex IdcardPattern();

    /// <summary>
    /// 中国大陆手机号码正则（13x ~ 19x 号段）
    /// </summary>
    /// <returns>编译生成的 Regex 实例</returns>
    [GeneratedRegex(@"^1[3-9]\d{9}$")]
    private static partial Regex MobilePattern();

    /// <summary>
    /// 电子邮箱地址正则（RFC 5322 简化版）
    /// </summary>
    /// <returns>编译生成的 Regex 实例</returns>
    [GeneratedRegex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
    private static partial Regex EmailPattern();

    /// <summary>
    /// 校验身份证号码是否合法（正则 + ISO 7064:1983.MOD 11-2 校验位算法）
    /// </summary>
    /// <param name="idcard">身份证号码字符串</param>
    /// <returns>合法返回 true；否则返回 false</returns>
    public static bool ValidateIdcard(string idcard)
    {
        if (!IdcardPattern().IsMatch(idcard)) return false;
        if (idcard.Length == 18)
        {
            // 前 17 位加权因子：从左到右依次为 2^17, 2^16, ..., 2^1 对 11 取模
            var factors = new[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
            // 预定义校验位字符表：sum % 11 作为索引取对应字符
            var parity = "10X98765432";
            var sum = 0;
            for (int i = 0; i < 17; i++)
                sum += (idcard[i] - '0') * factors[i];
            return parity[sum % 11] == char.ToUpper(idcard[17]);
        }
        return true;
    }

    /// <summary>
    /// 校验手机号格式是否合法
    /// </summary>
    /// <param name="mobile">手机号码字符串</param>
    /// <returns>合法返回 true；否则返回 false</returns>
    public static bool ValidateMobile(string mobile) => MobilePattern().IsMatch(mobile);

    /// <summary>
    /// 校验邮箱地址格式是否合法
    /// </summary>
    /// <param name="email">邮箱地址字符串</param>
    /// <returns>合法返回 true；否则返回 false</returns>
    public static bool ValidateEmail(string email) => EmailPattern().IsMatch(email);
}
