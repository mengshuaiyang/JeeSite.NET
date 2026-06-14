namespace JeeSiteNET.Core.Utils;

/// <summary>
/// 工作日计算工具类（过滤周六/周日，可选排除自定义节假日集合）
/// </summary>
public static class WorkDayUtil
{
    /// <summary>
    /// 计算给定时间段内的工作日数量（起止日期包含在内）
    /// </summary>
    /// <param name="start">起始日期（仅取 Date 部分）</param>
    /// <param name="end">结束日期（仅取 Date 部分）</param>
    /// <param name="holidays">可选的节假日集合；若日期包含在内则视为非工作日</param>
    /// <returns>工作日数量</returns>
    public static int GetWorkDays(DateTime start, DateTime end, HashSet<DateTime>? holidays = null)
    {
        var count = 0;
        // 按天遍历，排除节假日与周末
        for (var d = start.Date; d <= end.Date; d = d.AddDays(1))
        {
            if (holidays != null && holidays.Contains(d)) continue;
            if (d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday)
                count++;
        }
        return count;
    }

    /// <summary>
    /// 从给定日期起累加 N 个有效工作日，返回结果日期
    /// </summary>
    /// <param name="date">基准日期</param>
    /// <param name="days">要累加的工作日数（正整数）</param>
    /// <param name="holidays">可选的节假日集合；这些日期不计入工作日</param>
    /// <returns>经过 N 个工作日后的日期</returns>
    public static DateTime AddWorkDays(DateTime date, int days, HashSet<DateTime>? holidays = null)
    {
        var d = date;
        // 逐天向后推进，剩余天数仅在有效工作日时递减
        while (days > 0)
        {
            d = d.AddDays(1);
            if (holidays != null && holidays.Contains(d)) continue;
            if (d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday)
                days--;
        }
        return d;
    }
}
