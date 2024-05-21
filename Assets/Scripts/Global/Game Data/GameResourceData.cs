using System;

[Serializable]
public class GameResourceData
{
    public int Heart;
    public int Coins;
    public long HeartTime;

    public void AddCoins(int amount)
    {
        Coins += amount;
    }

    public void AddHeart(int amount)
    {
        Heart += amount;
    }

    public void SaveHeartTime(DateTime time)
    {
        HeartTime = TimeUtils.DateTimeToUnixMiliseconds(time);
    }

    public DateTime GetHeartTime()
    {
        return TimeUtils.UnixMilisecondsToDateTime(HeartTime);
    }
}
