    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Core.Security 命名空间
// 引入命名空间：JeeSiteNET.Core.Security
using JeeSiteNET.Core.Security;
    // 引入 JeeSiteNET.Modules.Sys.Application.DTOs 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.DTOs
using JeeSiteNET.Modules.Sys.Application.DTOs;
    // 引入 JeeSiteNET.Modules.Sys.Application.Services 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.Services
using JeeSiteNET.Modules.Sys.Application.Services;
    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;
    // 引入 Microsoft.AspNetCore.Mvc 命名空间
// 引入命名空间：Microsoft.AspNetCore.Mvc
using Microsoft.AspNetCore.Mvc;

// 定义 JeeSiteNET.Modules.Sys.Controllers 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Controllers
namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/biz-category")]
// 定义class BizCategoryController
// 定义类：BizCategoryController

public class BizCategoryController : ControllerBase
{
    // 字段 _bizCategoryService
    // 字段：_bizCategoryService

    private readonly BizCategoryService _bizCategoryService;
    // 构造函数 BizCategoryController
    // 构造函数：BizCategoryController

    public BizCategoryController(BizCategoryService bizCategoryService) => _bizCategoryService = bizCategoryService;

    [Permission("sys:biz-category:list")]
    [HttpGet("tree")]
    // 方法 Tree
    // 方法：Tree

    public async Task<ApiResult<List<BizCategoryDto>>> Tree()
        => ApiResult<List<BizCategoryDto>>.Ok(await _bizCategoryService.GetTreeAsync());

    [Permission("sys:biz-category:edit")]
    [HttpPost("save")]
    // 方法：Save

    public async Task<ApiResult> Save([FromBody] BizCategory entity) => await _bizCategoryService.SaveAsync(entity);

    [Permission("sys:biz-category:delete")]
    [HttpPost("delete")]
    // 方法：Delete

    public async Task<ApiResult> Delete([FromBody] DeleteBizCategoryRequest request) => await _bizCategoryService.DeleteAsync(request.CategoryCode);
}

// 定义class DeleteBizCategoryRequest
// 定义类：DeleteBizCategoryRequest

public class DeleteBizCategoryRequest { public string CategoryCode { get; set; } = string.Empty; }
