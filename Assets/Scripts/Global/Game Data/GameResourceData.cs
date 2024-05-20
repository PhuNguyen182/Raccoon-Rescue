using System;

[Serializable]
public class GameResourceData
{
    public int Heart;
    public int Coins;

    public void AddCoins(int amount)
    {
        Coins += amount;
    }

    public void AddHeart(int amount)
    {
        Heart += amount;
    }
}
