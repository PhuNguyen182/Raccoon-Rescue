using System;

public static class TimeUtils
{
    public static long GetCurrentTimestamp()
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

    public static bool IsNewDay(DateTime dateTime)
    {
        if (dateTime.Year > DateTime.Now.Year)
            return true;

        else if (dateTime.Month > DateTime.Now.Month)
            return true;

        else if (dateTime.Day > DateTime.Now.Day)
            return true;

        return false;
    }
}
