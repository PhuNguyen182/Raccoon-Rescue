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
            using(StreamReader reader = new(dataPath))
            {
                string json = reader.ReadToEnd();
                using(StringReader stringReader = new(json))
                {
                    using (JsonReader jsonReader = new JsonTextReader(stringReader))
                    {
                        JsonSerializer jsonSerializer = new();
                        T data = jsonSerializer.Deserialize<T>(jsonReader);
                        return data;
                    }
                }
            }
        }

        return default;
    }

    public static void SaveData(string name, T data)
    {
        string dataPath = $"{Application.persistentDataPath}/{name}.dat";

        using (StreamWriter writer = new(dataPath))
        {
            JsonSerializerSettings settings = new()
            {
                Formatting = Formatting.Indented,
            };

            string json = JsonConvert.SerializeObject(data, settings);
            writer.Write(json);
        }
    }

    public static void DeleteData(string name)
    {
        string dataPath = $"{Application.persistentDataPath}/{name}.dat";
        
        if (File.Exists(dataPath))
        {
            File.Delete(dataPath);
        }
    }
}
