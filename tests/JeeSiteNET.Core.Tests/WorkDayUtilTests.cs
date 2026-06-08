using JeeSiteNET.Core.Utils;
using FluentAssertions;

namespace JeeSiteNET.Core.Tests;

public class WorkDayUtilTests
{
    [Fact]
    public void GetWorkDays_MondayToFriday_Returns5()
    {
        var start = new DateTime(2024, 1, 8); // Monday
        var end = new DateTime(2024, 1, 12);  // Friday
        WorkDayUtil.GetWorkDays(start, end).Should().Be(5);
    }

    [Fact]
    public void GetWorkDays_FullWeek_Returns5()
    {
        var start = new DateTime(2024, 1, 8);  // Monday
        var end = new DateTime(2024, 1, 14);   // Sunday
        WorkDayUtil.GetWorkDays(start, end).Should().Be(5);
    }

    [Fact]
    public void GetWorkDays_SameDayWeekday_Returns1()
    {
        var date = new DateTime(2024, 1, 8); // Monday
        WorkDayUtil.GetWorkDays(date, date).Should().Be(1);
    }

    [Fact]
    public void GetWorkDays_SameDayWeekend_Returns0()
    {
        var date = new DateTime(2024, 1, 13); // Saturday
        WorkDayUtil.GetWorkDays(date, date).Should().Be(0);
    }

    [Fact]
    public void GetWorkDays_WithHoliday_ExcludesHoliday()
    {
        var start = new DateTime(2024, 1, 8); // Monday
        var end = new DateTime(2024, 1, 10);  // Wednesday
        var holidays = new HashSet<DateTime> { new(2024, 1, 9) }; // Tuesday is holiday
        WorkDayUtil.GetWorkDays(start, end, holidays).Should().Be(2); // Mon + Wed
    }

    [Fact]
    public void AddWorkDays_Add5DaysFromMonday_ReturnsNextMonday()
    {
        var start = new DateTime(2024, 1, 8); // Monday
        WorkDayUtil.AddWorkDays(start, 5).Should().Be(new DateTime(2024, 1, 15)); // Next Monday
    }

    [Fact]
    public void AddWorkDays_Add1DayOnFriday_ReturnsMonday()
    {
        var start = new DateTime(2024, 1, 12); // Friday
        WorkDayUtil.AddWorkDays(start, 1).Should().Be(new DateTime(2024, 1, 15)); // Monday
    }

    [Fact]
    public void AddWorkDays_WithHoliday_SkipsHoliday()
    {
        var start = new DateTime(2024, 1, 8); // Monday
        var holidays = new HashSet<DateTime> { new(2024, 1, 9) }; // Tuesday is holiday
        // Adding 1 work day: skip Tue(holiday), land on Wed
        WorkDayUtil.AddWorkDays(start, 1, holidays).Should().Be(new DateTime(2024, 1, 10));
    }
}
