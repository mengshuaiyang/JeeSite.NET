    // 引入 JeeSiteNET.Core.Utils 命名空间
// 引入命名空间：JeeSiteNET.Core.Utils
using JeeSiteNET.Core.Utils;
    // 引入 FluentAssertions 命名空间
// 引入命名空间：FluentAssertions
using FluentAssertions;

// 定义 JeeSiteNET.Core.Tests 命名空间
// 定义命名空间：JeeSiteNET.Core.Tests
namespace JeeSiteNET.Core.Tests;

// 定义class WorkDayUtilTests
// 定义类：WorkDayUtilTests
public class WorkDayUtilTests
{
    [Fact]
    // 方法 GetWorkDays_MondayToFriday_Returns5
    // 方法：GetWorkDays_MondayToFriday_Returns5
    public void GetWorkDays_MondayToFriday_Returns5()
    {
        // 创建 DateTime实例并赋给 start
        var start = new DateTime(2024, 1, 8); // Monday
        // 创建 DateTime实例并赋给 end
        var end = new DateTime(2024, 1, 12);  // Friday
        // 断言验证
        WorkDayUtil.GetWorkDays(start, end).Should().Be(5);
    }

    [Fact]
    // 方法 GetWorkDays_FullWeek_Returns5
    // 方法：GetWorkDays_FullWeek_Returns5
    public void GetWorkDays_FullWeek_Returns5()
    {
        // 创建 DateTime实例并赋给 start
        var start = new DateTime(2024, 1, 8);  // Monday
        // 创建 DateTime实例并赋给 end
        var end = new DateTime(2024, 1, 14);   // Sunday
        // 断言验证
        WorkDayUtil.GetWorkDays(start, end).Should().Be(5);
    }

    [Fact]
    // 方法 GetWorkDays_SameDayWeekday_Returns1
    // 方法：GetWorkDays_SameDayWeekday_Returns1
    public void GetWorkDays_SameDayWeekday_Returns1()
    {
        // 创建 DateTime实例并赋给 date
        var date = new DateTime(2024, 1, 8); // Monday
        // 断言验证
        WorkDayUtil.GetWorkDays(date, date).Should().Be(1);
    }

    [Fact]
    // 方法 GetWorkDays_SameDayWeekend_Returns0
    // 方法：GetWorkDays_SameDayWeekend_Returns0
    public void GetWorkDays_SameDayWeekend_Returns0()
    {
        // 创建 DateTime实例并赋给 date
        var date = new DateTime(2024, 1, 13); // Saturday
        // 断言验证
        WorkDayUtil.GetWorkDays(date, date).Should().Be(0);
    }

    [Fact]
    // 方法 GetWorkDays_WithHoliday_ExcludesHoliday
    // 方法：GetWorkDays_WithHoliday_ExcludesHoliday
    public void GetWorkDays_WithHoliday_ExcludesHoliday()
    {
        // 创建 DateTime实例并赋给 start
        var start = new DateTime(2024, 1, 8); // Monday
        // 创建 DateTime实例并赋给 end
        var end = new DateTime(2024, 1, 10);  // Wednesday
        // 创建 HashSet实例并赋给 holidays
        var holidays = new HashSet<DateTime> { new(2024, 1, 9) }; // Tuesday is holiday
        // 断言验证
        WorkDayUtil.GetWorkDays(start, end, holidays).Should().Be(2); // Mon + Wed
    }

    [Fact]
    // 方法 AddWorkDays_Add5DaysFromMonday_ReturnsNextMonday
    // 方法：AddWorkDays_Add5DaysFromMonday_ReturnsNextMonday
    public void AddWorkDays_Add5DaysFromMonday_ReturnsNextMonday()
    {
        // 创建 DateTime实例并赋给 start
        var start = new DateTime(2024, 1, 8); // Monday
        // 断言验证
        WorkDayUtil.AddWorkDays(start, 5).Should().Be(new DateTime(2024, 1, 15)); // Next Monday
    }

    [Fact]
    // 方法 AddWorkDays_Add1DayOnFriday_ReturnsMonday
    // 方法：AddWorkDays_Add1DayOnFriday_ReturnsMonday
    public void AddWorkDays_Add1DayOnFriday_ReturnsMonday()
    {
        // 创建 DateTime实例并赋给 start
        var start = new DateTime(2024, 1, 12); // Friday
        // 断言验证
        WorkDayUtil.AddWorkDays(start, 1).Should().Be(new DateTime(2024, 1, 15)); // Monday
    }

    [Fact]
    // 方法 AddWorkDays_WithHoliday_SkipsHoliday
    // 方法：AddWorkDays_WithHoliday_SkipsHoliday
    public void AddWorkDays_WithHoliday_SkipsHoliday()
    {
        // 创建 DateTime实例并赋给 start
        var start = new DateTime(2024, 1, 8); // Monday
        // 创建 HashSet实例并赋给 holidays
        var holidays = new HashSet<DateTime> { new(2024, 1, 9) }; // Tuesday is holiday
        // Adding 1 work day: skip Tue(holiday), land on Wed
        // 断言验证
        WorkDayUtil.AddWorkDays(start, 1, holidays).Should().Be(new DateTime(2024, 1, 10));
    }
}
