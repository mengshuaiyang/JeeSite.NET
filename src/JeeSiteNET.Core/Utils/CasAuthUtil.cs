using System.Net;
using System.Xml.Linq;

namespace JeeSiteNET.Core.Utils;

/// <summary>
/// CAS（Central Authentication Service）单点登录票据验证工具类
/// </summary>
public static class CasAuthUtil
{
    /// <summary>
    /// CAS 协议 XML 命名空间（耶鲁大学 CAS 标准）
    /// </summary>
    private static readonly XNamespace CasNs = "http://www.yale.edu/tp/cas";

    /// <summary>
    /// 内部共享 HttpClient 实例（单例模式，避免频繁创建释放导致的 Socket 耗尽）
    /// </summary>
    private static readonly HttpClient _httpClient = new() { Timeout = TimeSpan.FromSeconds(10) };

    /// <summary>
    /// 同步验证 CAS 登录票据（serviceValidate 协议）
    /// </summary>
    /// <param name="casServerUrl">CAS 服务器根地址（如 https://cas.example.com/cas）</param>
    /// <param name="ticket">登录成功后返回的 ST（Service Ticket）</param>
    /// <param name="serviceUrl">当前业务系统的 Service 地址（需与登录时提交一致）</param>
    /// <returns>验证结果对象；验证失败或网络异常返回 null</returns>
    public static CasValidateResult? ValidateTicket(string casServerUrl, string ticket, string serviceUrl)
    {
        // 拼接 CAS serviceValidate 接口地址，对 ticket 和 service 参数进行 URL 编码
        var validateUrl = $"{casServerUrl.TrimEnd('/')}/serviceValidate?ticket={WebUtility.UrlEncode(ticket)}&service={WebUtility.UrlEncode(serviceUrl)}";

        try
        {
            var response = _httpClient.GetStringAsync(validateUrl).GetAwaiter().GetResult();
            return ParseResponse(response);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 异步验证 CAS 登录票据（serviceValidate 协议）
    /// </summary>
    /// <param name="casServerUrl">CAS 服务器根地址（如 https://cas.example.com/cas）</param>
    /// <param name="ticket">登录成功后返回的 ST（Service Ticket）</param>
    /// <param name="serviceUrl">当前业务系统的 Service 地址（需与登录时提交一致）</param>
    /// <returns>验证结果对象；验证失败或网络异常返回 null</returns>
    public static async Task<CasValidateResult?> ValidateTicketAsync(string casServerUrl, string ticket, string serviceUrl)
    {
        var validateUrl = $"{casServerUrl.TrimEnd('/')}/serviceValidate?ticket={WebUtility.UrlEncode(ticket)}&service={WebUtility.UrlEncode(serviceUrl)}";

        try
        {
            var response = await _httpClient.GetStringAsync(validateUrl);
            return ParseResponse(response);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 解析 CAS serviceValidate 返回的 XML 响应文档
    /// </summary>
    /// <param name="xml">CAS 服务器返回的 XML 字符串</param>
    /// <returns>结构化的验证结果对象</returns>
    private static CasValidateResult? ParseResponse(string xml)
    {
        var doc = XDocument.Parse(xml);
        var success = doc.Descendants(CasNs + "authenticationSuccess").FirstOrDefault();
        if (success == null)
        {
            // 认证失败：读取 authenticationFailure 节点的 code 属性与错误消息
            var failure = doc.Descendants(CasNs + "authenticationFailure").FirstOrDefault();
            return new CasValidateResult
            {
                IsSuccess = false,
                FailureCode = failure?.Attribute("code")?.Value,
                FailureMessage = failure?.Value?.Trim()
            };
        }

        // 认证成功：读取用户名与自定义属性集合
        var username = success.Element(CasNs + "user")?.Value ?? "";
        var attributes = new Dictionary<string, string>();

        // 标准 CAS attributes 节点（命名空间为 CasNs 或无命名空间）
        var attrElem = success.Element(CasNs + "attributes");
        if (attrElem != null)
        {
            foreach (var el in attrElem.Elements())
            {
                var localName = el.Name.LocalName;
                if (el.Name.Namespace == CasNs || el.Name.Namespace == XNamespace.None)
                    attributes[localName] = el.Value;
            }
        }

        // 兼容部分 CAS 服务器使用 "cas:attributes" 前缀的写法
        var casAttrElem = success.Element(CasNs + "cas:attributes");
        if (casAttrElem != null)
        {
            foreach (var el in casAttrElem.Elements())
                attributes[el.Name.LocalName] = el.Value;
        }

        return new CasValidateResult
        {
            IsSuccess = true,
            Username = username,
            Attributes = attributes
        };
    }
}

/// <summary>
/// CAS 票据验证结果
/// </summary>
public class CasValidateResult
{
    /// <summary>
    /// 验证是否成功
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// 验证成功时返回的登录用户名
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// 验证失败时返回的错误代码（如 INVALID_TICKET）
    /// </summary>
    public string? FailureCode { get; set; }

    /// <summary>
    /// 验证失败时返回的错误描述
    /// </summary>
    public string? FailureMessage { get; set; }

    /// <summary>
    /// CAS 服务器返回的扩展属性集合
    /// </summary>
    public Dictionary<string, string> Attributes { get; set; } = [];
}
