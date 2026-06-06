namespace JeeSiteNET.Modules.Sys.Application.DTOs;

public class LoginDto
{
    public string LoginCode { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginResultDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
    public UserDto User { get; set; } = null!;
}
