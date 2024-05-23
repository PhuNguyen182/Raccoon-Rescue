using System;
using BubbleShooter.Scripts.Common.Enums;
using BubbleShooter.Scripts.Mainhome.Player;

[Serializable]
public class GameData : SingletonClass<GameData>
{
    private GameResourceData _gameResourceData;
    private InGameBoosterData _inGameBoosterData;
    private LevelProgressData _levelProgressData;

    private const string GameResourceDataKey = "GameResourceData";
    private const string InGameBoosterDataKey = "InGameBoosterData";
    private const string LevelProgressDataKey = "LevelProgressData";

    public ShopProfiler ShopProfiler { get; private set; }

    public GameData()
    {
        ShopProfiler = new();
    }

    public void LoadData()
    {
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
