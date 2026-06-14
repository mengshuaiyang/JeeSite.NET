    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Modules.Sys.Application.DTOs 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.DTOs
using JeeSiteNET.Modules.Sys.Application.DTOs;
    // 引入 JeeSiteNET.Modules.Sys.Application.Services 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.Services
using JeeSiteNET.Modules.Sys.Application.Services;
    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;
    // 引入 JeeSiteNET.Core.Security 命名空间
// 引入命名空间：JeeSiteNET.Core.Security
using JeeSiteNET.Core.Security;
    // 引入 Microsoft.AspNetCore.Authorization 命名空间
// 引入命名空间：Microsoft.AspNetCore.Authorization
using Microsoft.AspNetCore.Authorization;
    // 引入 Microsoft.AspNetCore.Mvc 命名空间
// 引入命名空间：Microsoft.AspNetCore.Mvc
using Microsoft.AspNetCore.Mvc;

// 定义 JeeSiteNET.Modules.Sys.Controllers 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Controllers
namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/config")]
// 定义class ConfigController
// 定义类：ConfigController

public class ConfigController : ControllerBase
{
    // 字段 _configService
    // 字段：_configService

    private readonly ConfigService _configService;
    // 构造函数 ConfigController
    // 构造函数：ConfigController

    public ConfigController(ConfigService configService) => _configService = configService;

    [Permission("sys:config:list")]
    [HttpPost("list")]
    // 方法：List

    public async Task<ApiResult<PageResult<ConfigDto>>> List([FromBody] PageRequest<Config> request)
    {
        // return 返回结果
        return ApiResult<PageResult<ConfigDto>>.Ok(await _configService.FindPageAsync(request));
    }

    [Permission("sys:config:list")]
    [HttpGet("get")]
    // 方法 Get
    // 方法：Get

    public async Task<ApiResult<ConfigDto?>> Get([FromQuery] string configKey)
    {
        // 缓存：获取值
        var entity = await _configService.GetAsync(configKey);
        // return 返回结果
        return entity == null ? ApiResult<ConfigDto?>.NotFound("配置不存在") : ApiResult<ConfigDto?>.Ok(entity);
    }

    [Permission("sys:config:edit")]
    [HttpPost("save")]
    // 方法：Save

    public async Task<ApiResult> Save([FromBody] ConfigSaveDto dto) => await _configService.SaveAsync(dto);

    [Permission("sys:config:delete")]
    [HttpPost("delete")]
    // 方法：Delete

    public async Task<ApiResult> Delete([FromBody] DeleteConfigRequest request) => await _configService.DeleteAsync(request.ConfigKey);
}

// 定义class DeleteConfigRequest
// 定义类：DeleteConfigRequest

public class DeleteConfigRequest { public string ConfigKey { get; set; } = string.Empty; }
