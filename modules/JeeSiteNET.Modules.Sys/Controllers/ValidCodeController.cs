using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZiggyCreatures.Caching.Fusion;

namespace JeeSiteNET.Modules.Sys.Controllers;

/// <summary>图形验证码接口控制器，负责生成图片验证码并将答案存入 FusionCache，提供校验接口。</summary>
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

    /// <summary>HTTP GET - 生成图形验证码图片并将答案存入缓存，返回 PNG 图片响应。</summary>
    [HttpGet("image/{key}")]
    public async Task<IActionResult> GetImage(string key)
    {
        var code = GenerateCode();

        await _cache.SetAsync($"Captcha:{key}", code, TimeSpan.FromMinutes(5));

        var bytes = CaptchaUtil.GenerateImage(code);

        return File(bytes, "image/png");
    }

    /// <summary>HTTP GET - 校验用户输入的图形验证码是否正确。</summary>
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
