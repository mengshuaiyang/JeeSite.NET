using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/lang")]
public class LangController : ControllerBase
{
    private readonly LangService _langService;
    public LangController(LangService langService) => _langService = langService;

    [Permission("sys:lang:list")]
    [HttpGet("list")]
    public async Task<ApiResult<List<LangDto>>> List()
        => ApiResult<List<LangDto>>.Ok(await _langService.GetAllAsync());

    [AllowAnonymous]
    [HttpGet("get-by-type")]
    public async Task<ApiResult<List<LangDto>>> GetByType([FromQuery] string langType)
        => ApiResult<List<LangDto>>.Ok(await _langService.GetByLangTypeAsync(langType));

    [Permission("sys:lang:edit")]
    [HttpPost("save")]
    public async Task<ApiResult> Save([FromBody] LangSaveDto dto) => await _langService.SaveAsync(dto);

    [Permission("sys:lang:delete")]
    [HttpPost("delete")]
    public async Task<ApiResult> Delete([FromBody] DeleteLangRequest request) => await _langService.DeleteAsync(request.Id);
}

public class DeleteLangRequest { public string Id { get; set; } = string.Empty; }
