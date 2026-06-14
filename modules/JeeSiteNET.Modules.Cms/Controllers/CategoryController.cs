using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Modules.Cms.Application.DTOs;
using JeeSiteNET.Modules.Cms.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Cms.Controllers;

/// <summary>CMS 栏目管理接口控制器，提供栏目列表、树形结构、详情、保存、删除等接口。</summary>
[ApiController]
[Route("api/v1/cms/category")]
public class CategoryController : ControllerBase

{
    private readonly CategoryService _categoryService;

    public CategoryController(CategoryService categoryService) => _categoryService = categoryService;

    /// <summary>HTTP POST - 分页查询列表，支持筛选与排序条件。</summary>
    [AllowAnonymous]
    [HttpPost("list")]
    public async Task<ApiResult<List<CategoryDto>>> List()

        => ApiResult<List<CategoryDto>>.Ok(await _categoryService.FindListAsync());

    /// <summary>HTTP GET - 获取树形结构数据，用于前端树形组件。</summary>
    [AllowAnonymous]
    [HttpGet("tree")]
    public async Task<ApiResult<List<CategoryDto>>> Tree()

        => ApiResult<List<CategoryDto>>.Ok(await _categoryService.FindTreeAsync());

    /// <summary>HTTP GET - 根据主键获取单条详情。</summary>
    [AllowAnonymous]
    [HttpGet("get")]
    public async Task<ApiResult<CategoryDto?>> Get([FromQuery] string categoryCode)
    {
        var dto = await _categoryService.GetAsync(categoryCode);

        return dto == null ? ApiResult<CategoryDto?>.NotFound("栏目不存在") : ApiResult<CategoryDto?>.Ok(dto);
    }

    /// <summary>HTTP POST - 新增或更新实体信息。</summary>
    [Permission("cms:category:edit")]
    [HttpPost("save")]
    public async Task<ApiResult> Save([FromBody] CategorySaveDto dto) => await _categoryService.SaveAsync(dto);

    /// <summary>HTTP POST - 删除指定实体（软删或硬删）。</summary>
    [Permission("cms:category:delete")]
    [HttpPost("delete")]
    public async Task<ApiResult> Delete([FromBody] DeleteCategoryRequest request) => await _categoryService.DeleteAsync(request.CategoryCode);
}

/// <summary>栏目删除请求 DTO。</summary>

public class DeleteCategoryRequest { public string CategoryCode { get; set; } = string.Empty; }
