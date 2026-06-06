namespace JeeSiteNET.Core.Utils;

public static class IdGenerator
{
    private static long _lastTimestamp = -1;
    private static long _sequence = 0;
    private static readonly object _lock = new();

    public static string NewId()
    {
        lock (_lock)
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
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
