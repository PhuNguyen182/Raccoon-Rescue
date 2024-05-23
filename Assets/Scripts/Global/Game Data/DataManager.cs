public static class DataManager
{
    private const string DataName = "GameData";

    public static void LoadData()
    {
        GameData gameData = SimpleSaveSystem<GameData>.LoadData(DataName);
        GameData.Instance.Initialize(gameData);
    }

    public static void SaveData()
    {
        SimpleSaveSystem<GameData>.SaveData(DataName, GameData.Instance);
    }

    public static void DeleteData()
    {
        SimpleSaveSystem<GameData>.DeleteData(DataName);
    }
}
