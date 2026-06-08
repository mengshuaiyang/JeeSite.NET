using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZiggyCreatures.Caching.Fusion;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/validCode")]
[AllowAnonymous]
public class ValidCodeController : ControllerBase
{
    private readonly IFusionCache _cache;
    
    public ValidCodeController(IFusionCache cache)
    {
        _cache = cache;
    }

    [HttpGet("image/{key}")]
    public async Task<IActionResult> GetImage(string key)
    {
        var code = GenerateCode();
        await _cache.SetAsync($"Captcha:{key}", code, TimeSpan.FromMinutes(5));
        var bytes = CaptchaUtil.GenerateImage(code);
        return File(bytes, "image/png");
    }

    [HttpGet("validate/{key}")]
    public async Task<ApiResult> Validate(string key, [FromQuery] string code)
    {
        var cached = await _cache.GetOrDefaultAsync<string>($"Captcha:{key}");
        if (string.IsNullOrEmpty(cached))
            return ApiResult.Fail(400, "验证码已过期");
        if (!string.Equals(cached, code, StringComparison.OrdinalIgnoreCase))
            return ApiResult.Fail(400, "验证码错误");
        await _cache.RemoveAsync($"Captcha:{key}");
        return ApiResult.Ok();
    }

    private static string GenerateCode()
    {
        var chars = "ABDEFGHKMNRSWX2345689".ToCharArray();
        var code = new char[4];
        var random = Random.Shared;
        for (int i = 0; i < 4; i++)
            code[i] = chars[random.Next(chars.Length)];
        return new string(code);
    }
}
