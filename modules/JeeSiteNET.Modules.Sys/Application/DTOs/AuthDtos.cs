namespace JeeSiteNET.Modules.Sys.Application.DTOs;

public class RegisterDto
{
    public string LoginCode { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
}

public class ForgotPasswordDto
{
    public string LoginCode { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class ResetPasswordDto
{
    public string Token { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
