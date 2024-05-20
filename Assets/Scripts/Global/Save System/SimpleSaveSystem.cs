using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public static class SimpleSaveSystem<T>
{
    public static T LoadData(string name)
    {
        string dataPath = $"{Application.persistentDataPath}/{name}.dat";

        if (File.Exists(dataPath))
        {
            using(StreamReader reader = new StreamReader(dataPath))
            {
                string json = reader.ReadLine();
                T data = JsonConvert.DeserializeObject<T>(json);
                return data;
            }
        }

        return default;
    }

    public static void SaveData(string name, T data)
    {
        string dataPath = $"{Application.persistentDataPath}/{name}.dat";

        using (StreamWriter writer = new StreamWriter(dataPath))
        {
            string json = JsonConvert.SerializeObject(data);
            writer.WriteLine(json);
        }
    }
}
