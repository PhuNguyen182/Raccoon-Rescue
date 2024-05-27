public static class DataManager
{
    public static void LoadData()
    {
        GameData.Instance.LoadData();
    }

    public static void SaveData()
    {
        GameData.Instance.SaveData();
    }

    public static void DeleteData()
    {
        GameData.Instance.DeleteData();
    }
}
