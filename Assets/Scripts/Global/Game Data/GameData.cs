using System;
using BubbleShooter.Scripts.Common.Enums;
using Scripts.Service;

[Serializable]
public class GameData : SingletonClass<GameData>
{
    public GameResourceData GameResourceData;
    public InGameBoosterData InGameBoosterData;
    public LevelProgressData LevelProgressData;

    public void Initialize(GameData gameData)
    {
        if (gameData != null)
        {
            GameResourceData = gameData.GameResourceData;
            InGameBoosterData = gameData.InGameBoosterData;
            LevelProgressData = gameData.LevelProgressData;
        }
    }

    public GameData()
    {
        GameResourceData = new();
        InGameBoosterData = new(0, 0, 0);
        LevelProgressData = new(new());
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
