using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public static class DataManager
{
    public static void Load()
    {
        GameData.Instance.Initialize();
    }

    public static void Save()
    {
        string dataJson = JsonConvert.SerializeObject(GameData.Instance);
    }
}
