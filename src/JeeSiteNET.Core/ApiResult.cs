namespace JeeSiteNET.Core;

/// <summary>
/// 通用泛型 API 统一响应结果
/// </summary>
/// <typeparam name="T">响应数据的类型</typeparam>
public class ApiResult<T>
{
    /// <summary>
    /// 响应状态码（200=成功，4xx=客户端错误，5xx=服务端错误）
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// 响应消息文本
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// 响应业务数据
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// 创建成功响应
    /// </summary>
    /// <param name="data">业务数据</param>
    /// <param name="message">成功消息</param>
    /// <returns>状态码 200 的成功响应</returns>
    public static ApiResult<T> Ok(T? data = default, string message = "操作成功")
        => new() { Code = 200, Message = message, Data = data };

    /// <summary>
    /// 创建失败响应
    /// </summary>
    /// <param name="code">错误状态码（默认 400）</param>
    /// <param name="message">错误消息</param>
    /// <returns>状态码 4xx 的失败响应</returns>
    public static ApiResult<T> Fail(int code = 400, string message = "操作失败")
        => new() { Code = code, Message = message };

    /// <summary>
    /// 创建未认证响应（401）
    /// </summary>
    /// <param name="message">未认证消息</param>
    /// <returns>状态码 401 的未认证响应</returns>
    public static ApiResult<T> Unauthorized(string message = "未登录或登录已过期")
        => new() { Code = 401, Message = message };

    /// <summary>
    /// 创建无权限响应（403）
    /// </summary>
    /// <param name="message">无权限消息</param>
    /// <returns>状态码 403 的无权限响应</returns>
    public static ApiResult<T> Forbidden(string message = "无操作权限")
        => new() { Code = 403, Message = message };

    /// <summary>
    /// 创建资源不存在响应（404）
    /// </summary>
    /// <param name="message">资源不存在消息</param>
    /// <returns>状态码 404 的资源不存在响应</returns>
    public static ApiResult<T> NotFound(string message = "资源不存在")
        => new() { Code = 404, Message = message };

    /// <summary>
    /// 创建服务器内部错误响应（500）
    /// </summary>
    /// <param name="message">错误消息</param>
    /// <returns>状态码 500 的服务器内部错误响应</returns>
    public static ApiResult<T> Error(string message = "服务器内部错误")
        => new() { Code = 500, Message = message };
}

/// <summary>
/// 通用非泛型 API 统一响应结果（Data 为 object 类型）
/// </summary>
public class ApiResult
{
    /// <summary>
    /// 响应状态码（200=成功，4xx=客户端错误，5xx=服务端错误）
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// 响应消息文本
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// 响应业务数据（object 类型）
    /// </summary>
    public object? Data { get; set; }

    /// <summary>
    /// 创建成功响应
    /// </summary>
    /// <param name="data">业务数据</param>
    /// <param name="message">成功消息</param>
    /// <returns>状态码 200 的成功响应</returns>
    public static ApiResult Ok(object? data = null, string message = "操作成功")
        => new() { Code = 200, Message = message, Data = data };

    /// <summary>
    /// 创建失败响应
    /// </summary>
    /// <param name="code">错误状态码（默认 400）</param>
    /// <param name="message">错误消息</param>
    /// <returns>状态码 4xx 的失败响应</returns>
    public static ApiResult Fail(int code = 400, string message = "操作失败")
        => new() { Code = code, Message = message };

    /// <summary>
    /// 创建未认证响应（401）
    /// </summary>
    /// <param name="message">未认证消息</param>
    /// <returns>状态码 401 的未认证响应</returns>
    public static ApiResult Unauthorized(string message = "未登录或登录已过期")
        => new() { Code = 401, Message = message };

    /// <summary>
    /// 创建无权限响应（403）
    /// </summary>
    /// <param name="message">无权限消息</param>
    /// <returns>状态码 403 的无权限响应</returns>
    public static ApiResult Forbidden(string message = "无操作权限")
        => new() { Code = 403, Message = message };

    /// <summary>
    /// 创建资源不存在响应（404）
    /// </summary>
    /// <param name="message">资源不存在消息</param>
    /// <returns>状态码 404 的资源不存在响应</returns>
    public static ApiResult NotFound(string message = "资源不存在")
        => new() { Code = 404, Message = message };

    /// <summary>
    /// 创建服务器内部错误响应（500）
    /// </summary>
    /// <param name="message">错误消息</param>
    /// <returns>状态码 500 的服务器内部错误响应</returns>
    public static ApiResult Error(string message = "服务器内部错误")
        => new() { Code = 500, Message = message };
}
