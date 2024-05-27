using System;

[Serializable]
public class GameResourceData
{
    public int Heart;
    public int Coins;
    public long HeartTime;

    public GameResourceData()
    {
        Coins = 0;
        Heart = GameDataConstants.MaxHeart;
        HeartTime = TimeUtils.DatetimeToBinary(DateTime.MinValue);
    }

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
        HeartTime = TimeUtils.DatetimeToBinary(time);
    }

    public DateTime GetHeartTime()
    {
        return TimeUtils.BinaryToDatetime(HeartTime);
    }
}
