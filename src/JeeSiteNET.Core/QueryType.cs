namespace JeeSiteNET.Core;

public enum QueryType
{
    EQ,         // 等于
    NE,         // 不等于
    LIKE,       // 模糊匹配
    RIGHT_LIKE, // 右模糊
    LEFT_LIKE,  // 左模糊
    GT,         // 大于
    GE,         // 大于等于
    LT,         // 小于
    LE,         // 小于等于
    BETWEEN,    // 范围
    IN,         // 包含
    NOT_IN,     // 不包含
    IS_NULL,    // 为空
    NOT_NULL    // 不为空
}
