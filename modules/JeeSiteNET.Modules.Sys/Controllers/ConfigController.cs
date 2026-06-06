using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Application.Services;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Core.Security;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/config")]
public class ConfigController : ControllerBase
{
    private readonly ConfigService _configService;
    public ConfigController(ConfigService configService) => _configService = configService;

    [HttpPost("list")]
    public async Task<ApiResult<PageResult<ConfigDto>>> List([FromBody] PageRequest<Config> request)
    {
        return ApiResult<PageResult<ConfigDto>>.Ok(await _configService.FindPageAsync(request));
    }

    [HttpGet("get")]
    public async Task<ApiResult<ConfigDto?>> Get([FromQuery] string configKey)
    {
        var entity = await _configService.GetAsync(configKey);
        return entity == null ? ApiResult<ConfigDto?>.NotFound("配置不存在") : ApiResult<ConfigDto?>.Ok(entity);
    }

    [HttpPost("save")]
    public async Task<ApiResult> Save([FromBody] ConfigSaveDto dto) => await _configService.SaveAsync(dto);

    [HttpPost("delete")]
    public async Task<ApiResult> Delete([FromBody] DeleteConfigRequest request) => await _configService.DeleteAsync(request.ConfigKey);
}

public class DeleteConfigRequest { public string ConfigKey { get; set; } = string.Empty; }
