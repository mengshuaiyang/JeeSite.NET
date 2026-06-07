using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Application.Services;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/biz-category")]
public class BizCategoryController : ControllerBase
{
    private readonly BizCategoryService _bizCategoryService;
    public BizCategoryController(BizCategoryService bizCategoryService) => _bizCategoryService = bizCategoryService;

    [Permission("sys:biz-category:list")]
    [HttpGet("tree")]
    public async Task<ApiResult<List<BizCategoryDto>>> Tree()
        => ApiResult<List<BizCategoryDto>>.Ok(await _bizCategoryService.GetTreeAsync());

    [Permission("sys:biz-category:edit")]
    [HttpPost("save")]
    public async Task<ApiResult> Save([FromBody] BizCategory entity) => await _bizCategoryService.SaveAsync(entity);

    [Permission("sys:biz-category:delete")]
    [HttpPost("delete")]
    public async Task<ApiResult> Delete([FromBody] DeleteBizCategoryRequest request) => await _bizCategoryService.DeleteAsync(request.CategoryCode);
}

public class DeleteBizCategoryRequest { public string CategoryCode { get; set; } = string.Empty; }
