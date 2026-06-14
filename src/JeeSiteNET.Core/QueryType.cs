namespace JeeSiteNET.Core;

/// <summary>
/// 动态查询条件类型枚举：用于动态构建查询表达式时标识操作符
/// </summary>
public enum QueryType
{
    /// <summary>等于</summary>
    EQ,
    /// <summary>不等于</summary>
    NE,
    /// <summary>模糊匹配（前后通配，即 LIKE '%value%'）</summary>
    LIKE,
    /// <summary>右模糊（前通配，即 LIKE '%value'）</summary>
    RIGHT_LIKE,
    /// <summary>左模糊（后通配，即 LIKE 'value%'）</summary>
    LEFT_LIKE,
    /// <summary>大于</summary>
    GT,
    /// <summary>大于等于</summary>
    GE,
    /// <summary>小于</summary>
    LT,
    /// <summary>小于等于</summary>
    LE,
    /// <summary>范围查询（介于两值之间）</summary>
    BETWEEN,
    /// <summary>包含（IN 子句）</summary>
    IN,
    /// <summary>不包含（NOT IN 子句）</summary>
    NOT_IN,
    /// <summary>为空</summary>
    IS_NULL,
    /// <summary>不为空</summary>
    NOT_NULL
}
