using System;
using System.Diagnostics;
using Scripts.Service;
using UnityEngine;
using Debug = UnityEngine.Debug;

[Serializable]
public class GameData : SingletonClass<GameData>, IService
{
    public InGameBoosterData InGameBoosterData;

    public void Initialize()
    {
        // If there is no saved data, construct new data profiles
        // otherwise the data will be loaded from saved position
        Debug.Log("Game Data Constructor");
    }
}
