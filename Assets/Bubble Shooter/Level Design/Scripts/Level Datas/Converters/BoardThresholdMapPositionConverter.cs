using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using BubbleShooter.LevelDesign.Scripts.LevelDatas.CustomDatas;
using System;
using BubbleShooter.LevelDesign.Scripts.JsonUtils;

namespace BubbleShooter.LevelDesign.Scripts.LevelDatas.Converters
{
    public class BoardThresholdMapPositionConverter : JsonConverter<BoardThresholdMapPosition>
    {
        public override BoardThresholdMapPosition ReadJson(JsonReader reader, Type objectType, BoardThresholdMapPosition existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if(reader.ReadInts(out int? x, out int? y))
            {
                reader.Read();
            }

            if(x.HasValue && y.HasValue)
            {
                return new BoardThresholdMapPosition
                {
                    Position = new Vector3Int(x.Value, y.Value)
                };
            }

            return existingValue;
        }

        public override void WriteJson(JsonWriter writer, BoardThresholdMapPosition value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            writer.WriteValue(value.Position.x);
            writer.WriteValue(value.Position.y);
            writer.WriteEndArray();
        }
    }
}
