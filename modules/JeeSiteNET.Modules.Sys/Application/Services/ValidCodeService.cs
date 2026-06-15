using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using ZiggyCreatures.Caching.Fusion;
using Microsoft.Extensions.Configuration;

namespace JeeSiteNET.Modules.Sys.Application.Services;

/// <summary>验证码服务：负责短信/邮件验证码的生成、发送、校验与限频（缓存 5 分钟有效，一次使用即失效）。</summary>
/// <remarks>限频策略：同目标同场景 60 秒内最多 1 次，10 分钟内最多 5 次，避免被刷短信/邮件通道。</remarks>
public class ValidCodeService
{
    private readonly IFusionCache _cache;
    private readonly IConfiguration _config;

    /// <summary>构造函数，注入缓存与配置服务。</summary>
    public ValidCodeService(IFusionCache cache, IConfiguration config)
    {
        _cache = cache;
        _config = config;
    }

    /// <summary>生成并发送验证码：根据 target 格式（含 @ 走邮件，否则走短信）选择通道。</summary>
    /// <param name="target">手机号或邮箱地址。</param>
    /// <param name="scene">业务场景（login / register / reset）。</param>
    public async Task<ApiResult> GenerateAndSendAsync(string target, string scene)
    {
        if (string.IsNullOrWhiteSpace(target)) return ApiResult.Fail(400, "请输入手机号或邮箱");
        scene = string.IsNullOrWhiteSpace(scene) ? "login" : scene.ToLowerInvariant();
        var allowed = new HashSet<string> { "login", "register", "reset" };
        if (!allowed.Contains(scene)) return ApiResult.Fail(400, "不支持的场景");

        // 60 秒最小间隔锁，防止前端重复点击刷屏
        var blockKey = $"ValidCode:Block:{scene}:{target}";
        if ((await _cache.TryGetAsync<bool>(blockKey)).HasValue)
            return ApiResult.Fail(400, "验证码发送过于频繁，请稍后再试");

        // 10 分钟 5 次频密计数
        var rateKey = $"ValidCode:Rate:{scene}:{target}";
        var rate = await _cache.GetOrDefaultAsync<int>(rateKey);
        if (rate >= 5) return ApiResult.Fail(400, "操作过于频繁，请稍后再试");

        // 生成 6 位数字验证码
        var code = Random.Shared.Next(100000, 999999).ToString();

        // 发送：根据 target 格式路由邮件 / 短信通道
        bool sentOk;
        if (target.Contains('@'))
        {
            var smtpHost = _config["Email:SmtpHost"] ?? "";
            var smtpPort = int.TryParse(_config["Email:SmtpPort"], out var p) ? p : 587;
            var smtpUser = _config["Email:Username"] ?? "";
            var smtpPwd = _config["Email:Password"] ?? "";
            if (string.IsNullOrWhiteSpace(smtpHost) || string.IsNullOrWhiteSpace(smtpUser))
                return ApiResult.Fail(400, "邮件服务未配置");
            try
            {
                sentOk = EmailUtil.Send(smtpHost, smtpPort, smtpUser, smtpPwd, target,
                    $"【验证码】{SceneLabel(scene)}",
                    $"您的验证码是 {code}，5 分钟内有效，请勿泄露给他人。");
            }
            catch
            {
                sentOk = false;
            }
        }
        else
        {
            var apiUrl = _config["Sms:ApiUrl"] ?? "";
            var apiKey = _config["Sms:ApiKey"] ?? "";
            if (string.IsNullOrWhiteSpace(apiUrl))
                return ApiResult.Fail(400, "短信服务未配置");
            sentOk = await SmsUtil.SendAsync(apiUrl, apiKey, target,
                $"【验证码】{code}，5 分钟内有效，请勿泄露给他人。");
        }

        if (!sentOk) return ApiResult.Fail(500, "验证码发送失败，请检查服务配置");

        // 存储验证码（5 分钟过期）
        var codeKey = $"ValidCode:{scene}:{target}";
        await _cache.SetAsync(codeKey, code, TimeSpan.FromMinutes(5));

        // 更新计数 + 60 秒最小间隔锁
        await _cache.SetAsync(rateKey, rate + 1, TimeSpan.FromMinutes(10));
        await _cache.SetAsync(blockKey, true, TimeSpan.FromSeconds(60));

        return ApiResult.Ok("验证码已发送，请查收");
    }

    /// <summary>校验验证码。校验成功后立即删除缓存，防止重放。</summary>
    public async Task<ApiResult> VerifyAsync(string target, string scene, string code)
    {
        if (string.IsNullOrWhiteSpace(target) || string.IsNullOrWhiteSpace(code))
            return ApiResult.Fail(400, "请输入手机号/邮箱和验证码");
        scene = string.IsNullOrWhiteSpace(scene) ? "login" : scene.ToLowerInvariant();

        var codeKey = $"ValidCode:{scene}:{target}";
        var cached = await _cache.GetOrDefaultAsync<string>(codeKey);
        if (string.IsNullOrEmpty(cached))
            return ApiResult.Fail(400, "验证码已过期或不存在，请重新获取");
        if (!string.Equals(cached, code, StringComparison.Ordinal))
            return ApiResult.Fail(400, "验证码错误");

        await _cache.RemoveAsync(codeKey);
        return ApiResult.Ok("验证成功");
    }

    /// <summary>静默校验验证码（不返回详细错误消息），供上层登录/重置流程使用。</summary>
    public async Task<bool> VerifySilentAsync(string target, string scene, string code)
    {
        if (string.IsNullOrWhiteSpace(target) || string.IsNullOrWhiteSpace(code)) return false;
        scene = string.IsNullOrWhiteSpace(scene) ? "login" : scene.ToLowerInvariant();
        var codeKey = $"ValidCode:{scene}:{target}";
        var cached = await _cache.GetOrDefaultAsync<string>(codeKey);
        if (string.IsNullOrEmpty(cached)) return false;
        if (!string.Equals(cached, code, StringComparison.Ordinal)) return false;
        await _cache.RemoveAsync(codeKey);
        return true;
    }

    /// <summary>将场景英文 key 映射为中文显示名，用于验证码内容提示。</summary>
    private static string SceneLabel(string scene) => scene switch
    {
        "login" => "登录",
        "register" => "注册",
        "reset" => "重置密码",
        _ => "身份校验"
    };
}
