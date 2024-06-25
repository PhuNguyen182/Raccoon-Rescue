using System;
using BubbleShooter.Scripts.Common.Enums;
using BubbleShooter.Scripts.Mainhome.Player;

public class GameData : SingletonClass<GameData>
{
    private GameResourceData _gameResourceData;
    private InGameBoosterData _inGameBoosterData;
    private LevelProgressData _levelProgressData;

    private const string GameResourceDataKey = "GameResourceData";
    private const string InGameBoosterDataKey = "InGameBoosterData";
    private const string LevelProgressDataKey = "LevelProgressData";

    public ShopProfiler ShopProfiler { get; private set; }
    public GameInventory GameInventory { get; private set; }

    public GameData()
    {
        ShopProfiler = new();
        GameInventory = new();
    }

    public void LoadData()
    {
        // The component data should be saved individually because GameData class doesn't support serialize
        _gameResourceData = SimpleSaveSystem<GameResourceData>.LoadData(GameResourceDataKey) ?? new();
        _inGameBoosterData = SimpleSaveSystem<InGameBoosterData>.LoadData(InGameBoosterDataKey) ?? new(0, 0, 0);
        _levelProgressData = SimpleSaveSystem<LevelProgressData>.LoadData(LevelProgressDataKey) ?? new(new());
    }

    public void SaveData()
    {
        SimpleSaveSystem<GameResourceData>.SaveData(GameResourceDataKey, _gameResourceData);
        SimpleSaveSystem<InGameBoosterData>.SaveData(InGameBoosterDataKey, _inGameBoosterData);
        SimpleSaveSystem<LevelProgressData>.SaveData(LevelProgressDataKey, _levelProgressData);
    }

    public void DeleteData()
    {
        SimpleSaveSystem<GameResourceData>.DeleteData(GameResourceDataKey);
        SimpleSaveSystem<InGameBoosterData>.DeleteData(InGameBoosterDataKey);
        SimpleSaveSystem<LevelProgressData>.DeleteData(LevelProgressDataKey);
    }

    public int GetCurrentLevel()
    {
        return _levelProgressData.Level;
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

    public int GetBooster(IngameBoosterType boosterType)
    {
        return boosterType switch
        {
            IngameBoosterType.Colorful => _inGameBoosterData.ColorfulCount,
            IngameBoosterType.PreciseAimer => _inGameBoosterData.TargetAimCount,
            IngameBoosterType.ChangeBall => _inGameBoosterData.RandomBallCount,
            _ => 0
        };
    }

    public void SetLevel(int level)
    {
        _levelProgressData.SetLevel(level);
    }

    public void AddLevelProgress(LevelProgress level)
    {
        _levelProgressData.Append(level);
    }

    public bool IsLevelComplete(int level)
    {
        return _levelProgressData.IsLevelComplete(level);
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
