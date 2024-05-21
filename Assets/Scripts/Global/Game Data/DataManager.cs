using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataManager
{
    public static void LoadData()
    {
        GameData gameData = SimpleSaveSystem<GameData>.LoadData("GameData");
        GameData.Instance.Initialize(gameData);
    }

    public static void SaveData()
    {
        SimpleSaveSystem<GameData>.SaveData("GameData", GameData.Instance);
    }
}
