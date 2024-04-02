using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace BubbleShooter.LevelDesign.Scripts.JsonUtils
{
    public static class JsonExtensions
    {
        public static void ReadInt(this JsonReader reader, out int? value)
        {
            value = null;

            try
            {
                value = reader.ReadAsInt32();
            }
            catch
            {
                Debug.LogError("Not An Integer!");
                return;
            }
        }

        public static bool ReadInts(this JsonReader reader, out int? x, out int? y)
        {
            x = y = 0;

            try
            {
                reader.ReadInt(out x);
                reader.ReadInt(out y);
                return true;
            }
            catch
            {
                Debug.LogError("Cannot read interger numbers!");
                return false;
            }
        }

        public static bool ReadInts(this JsonReader reader, out int? x, out int? y, out int? value1, out int? value2, out int? value3)
        {
            x = y = value1 = value2 = value3 = 0;

            try
            {
                reader.ReadInt(out x);
                reader.ReadInt(out y);
                reader.ReadInt(out value1);
                reader.ReadInt(out value2);
                reader.ReadInt(out value3);
                return true;
            }
            catch
            {
                Debug.LogError("Cannot read interger numbers!");
                return false;
            }
        }
    }
}
