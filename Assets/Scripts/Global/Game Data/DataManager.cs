using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataManager
{
    public static void Load()
    {
        GameData gameData = SimpleSaveSystem<GameData>.LoadData("GameData");
        GameData.Instance.Initialize(gameData);
    }

    public static void Save()
    {
        SimpleSaveSystem<GameData>.SaveData("GameData", GameData.Instance);
    }
}
