using System;
using BubbleShooter.Scripts.Common.Enums;
using BubbleShooter.Scripts.Mainhome.Player;

[Serializable]
public class GameData : SingletonClass<GameData>
{
    private GameResourceData _gameResourceData;
    private InGameBoosterData _inGameBoosterData;
    private LevelProgressData _levelProgressData;

    public ShopProfiler ShopProfiler { get; private set; }

    public void Initialize(GameData gameData)
    {
        if (gameData == null)
            return;

        _gameResourceData = gameData._gameResourceData;
        _inGameBoosterData = gameData._inGameBoosterData;
        _levelProgressData = gameData._levelProgressData;
    }

    public GameData()
    {
        _gameResourceData = new();
        _inGameBoosterData = new(0, 0, 0);
        _levelProgressData = new(new());

        ShopProfiler = new();
    }

    public void AddCoins(int amount)
    {
        _gameResourceData.AddCoins(amount);
    }

    public int GetCoins()
    {
        return _gameResourceData.Coins;
    }

    public void SpendCoins(int amount)
    {
        _gameResourceData.AddCoins(-amount);
    }

    public void AddHeart(int amount)
    {
        _gameResourceData.AddHeart(amount);
    }

    public int GetHeart()
    {
        return _gameResourceData.Heart;
    }

    public void SetHeart(int heart)
    {
        _gameResourceData.Heart = heart;
    }

    public void UseHeart(int amount)
    {
        _gameResourceData.AddHeart(-amount);
    }

    public void AddBooster(IngameBoosterType boosterType, int amount)
    {
        _inGameBoosterData.AddBooster(boosterType, amount);
    }

    public void UseBooster(IngameBoosterType boosterType, int amount)
    {
        _inGameBoosterData.AddBooster(boosterType, -amount);
    }

    public void AddLevelComplete(LevelProgress level)
    {
        _levelProgressData.Append(level);
    }

    public LevelProgress GetLevelProgress(int level)
    {
        return _levelProgressData.GetLevelProgress(level);
    }

    public DateTime GetCurrentHeartTime()
    {
        return _gameResourceData.GetHeartTime();
    }

    public void SaveHeartTime(DateTime time)
    {
        _gameResourceData.SaveHeartTime(time);
    }
}
