using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.LevelDesign.Scripts.LevelDatas.CustomDatas;
using BubbleShooter.LevelDesign.Scripts.JsonUtils;
using Newtonsoft.Json;

namespace BubbleShooter.LevelDesign.Scripts.LevelDatas.Converters
{
    public class CeilMapPositionConverter : JsonConverter<CeilMapPosition>
    {
        public override CeilMapPosition ReadJson(JsonReader reader, Type objectType, CeilMapPosition existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if(reader.ReadInts(out int? x, out int? y))
            {
                reader.Read();
            }

            if(x.HasValue && y.HasValue)
            {
                return new CeilMapPosition
                {
                    Position = new(x.Value, y.Value)
                };
            }

            return existingValue;
        }

        public override void WriteJson(JsonWriter writer, CeilMapPosition value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            writer.WriteValue(value.Position.x);
            writer.WriteValue(value.Position.y);
            writer.WriteEndArray();
        }
    }
}
