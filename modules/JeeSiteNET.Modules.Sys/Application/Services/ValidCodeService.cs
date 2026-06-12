using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using ZiggyCreatures.Caching.Fusion;
using Microsoft.Extensions.Configuration;

namespace JeeSiteNET.Modules.Sys.Application.Services;

public class ValidCodeService
{
    private readonly IFusionCache _cache;
    private readonly IConfiguration _config;

    public ValidCodeService(IFusionCache cache, IConfiguration config)
    {
        _cache = cache;
        _config = config;
    }

    /// <summary>生成并发送验证码。scene: login / register / reset。target: 手机号或邮箱</summary>
    public async Task<ApiResult> GenerateAndSendAsync(string target, string scene)
    {
        if (string.IsNullOrWhiteSpace(target)) return ApiResult.Fail(400, "请输入手机号或邮箱");
        scene = (scene ?? "login").ToLowerInvariant();
        var allowed = new HashSet<string> { "login", "register", "reset" };
        if (!allowed.Contains(scene)) return ApiResult.Fail(400, "不支持的场景");

        // ---- 限频：同一目标 1 分钟 1 次，10 分钟 5 次 ----
        var rateKey = $"ValidCode:Rate:{scene}:{target}";
        var rate = await _cache.GetOrDefaultAsync<int>(rateKey);
        if (rate >= 5) return ApiResult.Fail(400, "操作过于频繁，请稍后再试");

        // ---- 生成 6 位数字验证码 ----
        var code = Random.Shared.Next(100000, 999999).ToString();

        // ---- 发送（根据 target 格式判断短信或邮件）----
        bool sentOk;
        if (target.Contains('@'))
        {
            var smtpHost = _config["Email:SmtpHost"] ?? "";
            var smtpPort = int.TryParse(_config["Email:SmtpPort"], out var p) ? p : 587;
            var smtpUser = _config["Email:Username"] ?? "";
            var smtpPwd = _config["Email:Password"] ?? "";
            if (string.IsNullOrWhiteSpace(smtpHost) || string.IsNullOrWhiteSpace(smtpUser))
                return ApiResult.Fail(400, "邮件服务未配置");
            sentOk = EmailUtil.Send(smtpHost, smtpPort, smtpUser, smtpPwd, target,
                $"【验证码】{scene}", $"您的验证码是 {code}，5 分钟内有效。");
        }
        else
        {
            var apiUrl = _config["Sms:ApiUrl"] ?? "";
            var apiKey = _config["Sms:ApiKey"] ?? "";
            if (string.IsNullOrWhiteSpace(apiUrl))
                return ApiResult.Fail(400, "短信服务未配置");
            sentOk = await SmsUtil.SendAsync(apiUrl, apiKey, target, $"【验证码】{code}，5 分钟内有效。");
        }

        if (!sentOk) return ApiResult.Fail(500, "验证码发送失败，请检查服务配置");

        // ---- 存储验证码（5 分钟过期）----
        var codeKey = $"ValidCode:{scene}:{target}";
        await _cache.SetAsync(codeKey, code, TimeSpan.FromMinutes(5));

        // ---- 更新限频计数（1 分钟滑动）----
        var currentRate = await _cache.GetOrDefaultAsync<int>(rateKey);
        await _cache.SetAsync(rateKey, currentRate + 1, TimeSpan.FromMinutes(1));

        return ApiResult.Ok("验证码已发送，请查收");
    }

    /// <summary>校验验证码。校验成功后会立即删除缓存，不可复用。</summary>
    public async Task<ApiResult> VerifyAsync(string target, string scene, string code)
    {
        if (string.IsNullOrWhiteSpace(target) || string.IsNullOrWhiteSpace(code))
            return ApiResult.Fail(400, "请输入手机号/邮箱和验证码");

        var codeKey = $"ValidCode:{scene}:{target}";
        var cached = await _cache.GetOrDefaultAsync<string>(codeKey);
        if (string.IsNullOrEmpty(cached))
            return ApiResult.Fail(400, "验证码已过期或不存在，请重新获取");

        if (!string.Equals(cached, code, StringComparison.Ordinal))
            return ApiResult.Fail(400, "验证码错误");

        // 一次使用后立即删除
        await _cache.RemoveAsync(codeKey);
        return ApiResult.Ok("验证成功");
    }

    /// <summary>内部方法：校验验证码（不暴露错误细节，供登录流程使用）。</summary>
    public async Task<bool> VerifySilentAsync(string target, string scene, string code)
    {
        if (string.IsNullOrWhiteSpace(target) || string.IsNullOrWhiteSpace(code)) return false;
        var codeKey = $"ValidCode:{scene}:{target}";
        var cached = await _cache.GetOrDefaultAsync<string>(codeKey);
        if (string.IsNullOrEmpty(cached)) return false;
        if (!string.Equals(cached, code, StringComparison.Ordinal)) return false;
        await _cache.RemoveAsync(codeKey);
        return true;
    }
}
