using System;

public static class TimeConverter
{
    public static long CurrentTimestamp()
    {
        return DateTimeToUnix(DateTime.Now);
    }

    public static long DateTimeToUnix(DateTime dateTime)
    {
        return new DateTimeOffset(dateTime).ToUnixTimeSeconds();
    }

    public static DateTime UnixToDateTime(long timestamp)
    {
        DateTime epouch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return epouch.AddSeconds(timestamp).ToLocalTime();
    }
}
