using JeeSiteNET.Core;
using JeeSiteNET.Modules.Cms.Application.DTOs;
using JeeSiteNET.Modules.Cms.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Cms.Controllers;

[ApiController]
[Route("api/v1/cms/category")]
public class CategoryController : ControllerBase
{
    private readonly CategoryService _categoryService;
    public CategoryController(CategoryService categoryService) => _categoryService = categoryService;

    [HttpPost("list")]
    public async Task<ApiResult<List<CategoryDto>>> List()
        => ApiResult<List<CategoryDto>>.Ok(await _categoryService.FindListAsync());

    [HttpGet("tree")]
    public async Task<ApiResult<List<CategoryDto>>> Tree()
        => ApiResult<List<CategoryDto>>.Ok(await _categoryService.FindTreeAsync());

    [HttpGet("get")]
    public async Task<ApiResult<CategoryDto?>> Get([FromQuery] string categoryCode)
    {
        var dto = await _categoryService.GetAsync(categoryCode);
        return dto == null ? ApiResult<CategoryDto?>.NotFound("栏目不存在") : ApiResult<CategoryDto?>.Ok(dto);
    }

    [HttpPost("save")]
    public async Task<ApiResult> Save([FromBody] CategorySaveDto dto) => await _categoryService.SaveAsync(dto);

    [HttpPost("delete")]
    public async Task<ApiResult> Delete([FromBody] DeleteCategoryRequest request) => await _categoryService.DeleteAsync(request.CategoryCode);
}

public class DeleteCategoryRequest { public string CategoryCode { get; set; } = string.Empty; }
