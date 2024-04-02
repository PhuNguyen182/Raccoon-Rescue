using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using BubbleShooter.LevelDesign.Scripts.LevelDatas.CustomDatas;
using BubbleShooter.LevelDesign.Scripts.JsonUtils;

namespace BubbleShooter.LevelDesign.Scripts.LevelDatas.Converters
{
    public class BoardMapPositionConverter : JsonConverter<BoardMapPosition>
    {
        public override BoardMapPosition ReadJson(JsonReader reader, Type objectType, BoardMapPosition existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if(reader.ReadInts(out int? x, out int? y))
            {
                reader.Read();
            }

            if(x.HasValue && y.HasValue)
            {
                return new BoardMapPosition
                {
                    MapData = new(),
                    Position = new Vector3Int(x.Value, y.Value)
                };
            }

            return existingValue;
        }

        public override void WriteJson(JsonWriter writer, BoardMapPosition value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            writer.WriteValue(value.Position.x);
            writer.WriteValue(value.Position.y);
            writer.WriteEndArray();
        }
    }
}
