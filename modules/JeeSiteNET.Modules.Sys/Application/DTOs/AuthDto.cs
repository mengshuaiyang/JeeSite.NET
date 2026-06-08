namespace JeeSiteNET.Modules.Sys.Application.DTOs;

public class LoginDto
{
    public string LoginCode { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? ValidCode { get; set; }
    public string? ValidCodeKey { get; set; }
}

public class SmsDto
{
    public string PhoneNumber { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

public class EmailDto
{
    public string ToAddress { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string? Cc { get; set; }
    public string? Bcc { get; set; }
}

public class CasLoginDto
{
    public string Ticket { get; set; } = string.Empty;
    public string Service { get; set; } = string.Empty;
}

public class LoginResultDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
    public UserDto User { get; set; } = null!;
    public bool IsValidCodeLogin { get; set; }
}
