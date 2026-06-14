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
[Route("api/v1/sys/post")]
// 定义class PostController
// 定义类：PostController

public class PostController : ControllerBase
{
    // 字段 _postService
    // 字段：_postService

    private readonly PostService _postService;
    // 构造函数 PostController
    // 构造函数：PostController

    public PostController(PostService postService) => _postService = postService;

    [Permission("sys:post:list")]
    [HttpPost("list")]
    // 方法：List

    public async Task<ApiResult<PageResult<PostDto>>> List([FromBody] PageRequest<Post> request)
    {
        // return 返回结果
        return ApiResult<PageResult<PostDto>>.Ok(await _postService.FindPageAsync(request));
    }

    [Permission("sys:post:list")]
    [HttpGet("get")]
    // 方法 Get
    // 方法：Get

    public async Task<ApiResult<PostDto?>> Get([FromQuery] string postCode)
    {
        // 缓存：获取值
        var entity = await _postService.GetAsync(postCode);
        // return 返回结果
        return entity == null ? ApiResult<PostDto?>.NotFound("岗位不存在") : ApiResult<PostDto?>.Ok(entity);
    }

    [Permission("sys:post:edit")]
    [HttpPost("save")]
    // 方法：Save

    public async Task<ApiResult> Save([FromBody] PostSaveDto dto) => await _postService.SaveAsync(dto);

    [Permission("sys:post:delete")]
    [HttpPost("delete")]
    // 方法：Delete

    public async Task<ApiResult> Delete([FromBody] DeletePostRequest request) => await _postService.DeleteAsync(request.PostCode);
}

// 定义class DeletePostRequest
// 定义类：DeletePostRequest

public class DeletePostRequest { public string PostCode { get; set; } = string.Empty; }
