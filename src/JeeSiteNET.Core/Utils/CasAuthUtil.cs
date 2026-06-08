using System.Net;
using System.Xml.Linq;

namespace JeeSiteNET.Core.Utils;

public static class CasAuthUtil
{
    private static readonly XNamespace CasNs = "http://www.yale.edu/tp/cas";
    private static readonly HttpClient _httpClient = new() { Timeout = TimeSpan.FromSeconds(10) };

    public static CasValidateResult? ValidateTicket(string casServerUrl, string ticket, string serviceUrl)
    {
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

    private static CasValidateResult? ParseResponse(string xml)
    {
        var doc = XDocument.Parse(xml);
        var success = doc.Descendants(CasNs + "authenticationSuccess").FirstOrDefault();
        if (success == null)
        {
            var failure = doc.Descendants(CasNs + "authenticationFailure").FirstOrDefault();
            return new CasValidateResult
            {
                IsSuccess = false,
                FailureCode = failure?.Attribute("code")?.Value,
                FailureMessage = failure?.Value?.Trim()
            };
        }

        var username = success.Element(CasNs + "user")?.Value ?? "";
        var attributes = new Dictionary<string, string>();

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

public class CasValidateResult
{
    public bool IsSuccess { get; set; }
    public string? Username { get; set; }
    public string? FailureCode { get; set; }
    public string? FailureMessage { get; set; }
    public Dictionary<string, string> Attributes { get; set; } = [];
}
