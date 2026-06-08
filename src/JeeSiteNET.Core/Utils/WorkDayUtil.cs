namespace JeeSiteNET.Core.Utils;

public static class WorkDayUtil
{
    public static int GetWorkDays(DateTime start, DateTime end, HashSet<DateTime>? holidays = null)
    {
        var count = 0;
        for (var d = start.Date; d <= end.Date; d = d.AddDays(1))
        {
            if (holidays != null && holidays.Contains(d)) continue;
            if (d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday)
                count++;
        }
        return count;
    }

    public static DateTime AddWorkDays(DateTime date, int days, HashSet<DateTime>? holidays = null)
    {
        var d = date;
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
