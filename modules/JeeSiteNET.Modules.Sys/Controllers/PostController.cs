using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Application.Services;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/post")]
public class PostController : ControllerBase
{
    private readonly PostService _postService;
    public PostController(PostService postService) => _postService = postService;

    [HttpPost("list")]
    public async Task<ApiResult<PageResult<PostDto>>> List([FromBody] PageRequest<Post> request)
    {
        return ApiResult<PageResult<PostDto>>.Ok(await _postService.FindPageAsync(request));
    }

    [HttpGet("get")]
    public async Task<ApiResult<PostDto?>> Get([FromQuery] string postCode)
    {
        var entity = await _postService.GetAsync(postCode);
        return entity == null ? ApiResult<PostDto?>.NotFound("岗位不存在") : ApiResult<PostDto?>.Ok(entity);
    }

    [HttpPost("save")]
    public async Task<ApiResult> Save([FromBody] PostSaveDto dto) => await _postService.SaveAsync(dto);

    [HttpPost("delete")]
    public async Task<ApiResult> Delete([FromBody] DeletePostRequest request) => await _postService.DeleteAsync(request.PostCode);
}

public class DeletePostRequest { public string PostCode { get; set; } = string.Empty; }
