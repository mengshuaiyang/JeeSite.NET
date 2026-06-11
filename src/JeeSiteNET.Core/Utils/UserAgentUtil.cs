using System.Text.RegularExpressions;

namespace JeeSiteNET.Core.Utils;

public static partial class UserAgentUtil
{
    public static string ParseBrowser(string userAgent)
    {
        if (string.IsNullOrEmpty(userAgent)) return "Unknown";
        if (userAgent.Contains("Edg/") || userAgent.Contains("Edge/")) return "Edge";
        if (userAgent.Contains("Chrome/") && !userAgent.Contains("Edg/")) return "Chrome";
        if (userAgent.Contains("Safari/") && !userAgent.Contains("Chrome/")) return "Safari";
        if (userAgent.Contains("Firefox/")) return "Firefox";
        if (userAgent.Contains("MSIE") || userAgent.Contains("Trident/")) return "IE";
        if (userAgent.Contains("Opera/") || userAgent.Contains("OPR/")) return "Opera";
        return "Other";
    }

    public static string ParseOS(string userAgent)
    {
        if (string.IsNullOrEmpty(userAgent)) return "Unknown";
        if (userAgent.Contains("Windows NT 10")) return "Windows 10";
        if (userAgent.Contains("Windows NT 11")) return "Windows 11";
        if (userAgent.Contains("Windows NT 6.3")) return "Windows 8.1";
        if (userAgent.Contains("Windows NT 6.2")) return "Windows 8";
        if (userAgent.Contains("Windows NT 6.1")) return "Windows 7";
        if (userAgent.Contains("Windows")) return "Windows";
        if (userAgent.Contains("Mac OS X")) return "macOS";
        if (userAgent.Contains("iPhone")) return "iOS";
        if (userAgent.Contains("iPad")) return "iOS";
        if (userAgent.Contains("Android")) return "Android";
        if (userAgent.Contains("Linux")) return "Linux";
        return "Other";
    }

    public static string ParseDevice(string userAgent)
    {
        if (string.IsNullOrEmpty(userAgent)) return "Unknown";
        if (userAgent.Contains("Mobile")) return "Mobile";
        if (userAgent.Contains("Tablet") || userAgent.Contains("iPad")) return "Tablet";
        if (userAgent.Contains("bot") || userAgent.Contains("Bot") || userAgent.Contains("spider") || userAgent.Contains("crawl")) return "Bot";
        if (userAgent.Contains("TV") || userAgent.Contains("Smart-TV")) return "TV";
        return "Desktop";
    }
}
