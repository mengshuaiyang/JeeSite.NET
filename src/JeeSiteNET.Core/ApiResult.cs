namespace JeeSiteNET.Core;

public class ApiResult<T>
{
    public int Code { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }

    public static ApiResult<T> Ok(T? data = default, string message = "操作成功")
        => new() { Code = 200, Message = message, Data = data };

    public static ApiResult<T> Fail(int code = 400, string message = "操作失败")
        => new() { Code = code, Message = message };

    public static ApiResult<T> Unauthorized(string message = "未登录或登录已过期")
        => new() { Code = 401, Message = message };

    public static ApiResult<T> Forbidden(string message = "无操作权限")
        => new() { Code = 403, Message = message };

    public static ApiResult<T> NotFound(string message = "资源不存在")
        => new() { Code = 404, Message = message };

    public static ApiResult<T> Error(string message = "服务器内部错误")
        => new() { Code = 500, Message = message };
}

public class ApiResult
{
    public int Code { get; set; }
    public string Message { get; set; } = string.Empty;
    public object? Data { get; set; }

    public static ApiResult Ok(object? data = null, string message = "操作成功")
        => new() { Code = 200, Message = message, Data = data };

    public static ApiResult Fail(int code = 400, string message = "操作失败")
        => new() { Code = code, Message = message };

    public static ApiResult Unauthorized(string message = "未登录或登录已过期")
        => new() { Code = 401, Message = message };

    public static ApiResult Forbidden(string message = "无操作权限")
        => new() { Code = 403, Message = message };

    public static ApiResult NotFound(string message = "资源不存在")
        => new() { Code = 404, Message = message };

    public static ApiResult Error(string message = "服务器内部错误")
        => new() { Code = 500, Message = message };
}
