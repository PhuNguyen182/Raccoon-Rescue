using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public static class DataManager
{
    public static void Load()
    {
        GameData gameData = SimpleSaveSystem<GameData>.LoadData("GameData");
        GameData.Instance.Initialize(gameData);
    }

    public static void Save()
    {
        string dataJson = JsonConvert.SerializeObject(GameData.Instance);
    }
}
