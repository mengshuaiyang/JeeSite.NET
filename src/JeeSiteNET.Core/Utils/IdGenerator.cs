namespace JeeSiteNET.Core.Utils;

/// <summary>
/// 分布式 ID 生成器（精简实现）：组合「毫秒时间戳 + 同毫秒内自增序号」，
/// 并通过 lock 保证进程内多线程安全。适用于中小规模系统，不依赖外部号段服务。
/// </summary>
public static class IdGenerator
{
    /// <summary>
    /// 最近一次生成 ID 时的 Unix 毫秒时间戳（检测同毫秒冲突用）。
    /// </summary>
    private static long _lastTimestamp = -1;

    /// <summary>
    /// 同毫秒内的自增序号（每进 1 毫秒会重置为 0）。
    /// </summary>
    private static long _sequence = 0;

    /// <summary>
    /// 互斥锁：保护 _lastTimestamp / _sequence 两个共享字段。
    /// </summary>
    private static readonly object _lock = new();

    /// <summary>
    /// 生成一个新的字符串 ID：由「13 位毫秒时间戳」+「4 位零补齐的序号」组成。
    /// </summary>
    /// <returns>形如 <c>17171234567890001</c> 的唯一字符串 ID。</returns>
    public static string NewId()
    {
        lock (_lock)
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            // 同一毫秒：序号自增并与 4095 做位与，形成 0-4095 的循环；
            // 不同毫秒：重置时间戳与序号，避免未来回拨时产生重复。
            if (timestamp == _lastTimestamp)
            {
                _sequence = (_sequence + 1) & 4095;
            }
            else
            {
                _lastTimestamp = timestamp;
                _sequence = 0;
            }

            return $"{timestamp}{_sequence:D4}";
        }
    }
}
