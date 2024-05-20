using System;
using BubbleShooter.Scripts.Common.Enums;
using Scripts.Service;

[Serializable]
public class GameData : SingletonClass<GameData>, IService
{
    public GameResourceData GameResourceData;
    public InGameBoosterData InGameBoosterData;
    public LevelProgressData LevelProgressData;

    public void Initialize()
    {
        
    }

    public void AddCoins(int amount)
    {
        GameResourceData.AddCoins(amount);
    }

    public void SpendCoins(int amount)
    {
        GameResourceData.AddCoins(-amount);
    }

    public void AddHeart(int amount)
    {
        GameResourceData.AddHeart(amount);
    }

    public void UseHeart(int amount)
    {
        GameResourceData.AddHeart(-amount);
    }

    public void AddBooster(IngameBoosterType boosterType, int amount)
    {
        InGameBoosterData.AddBooster(boosterType, amount);
    }

    public void UseBooster(IngameBoosterType boosterType, int amount)
    {
        InGameBoosterData.AddBooster(boosterType, -amount);
    }

    public void AddLevelComplete(LevelProgress level)
    {
        LevelProgressData.Append(level);
    }

    public LevelProgress GetLevelProgress(int level)
    {
        return LevelProgressData.GetLevelProgress(level);
    }
}
