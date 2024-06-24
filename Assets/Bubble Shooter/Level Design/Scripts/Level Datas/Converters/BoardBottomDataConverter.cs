using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.LevelDesign.Scripts.JsonUtils;
using BubbleShooter.LevelDesign.Scripts.LevelDatas.CustomDatas;
using Newtonsoft.Json;

namespace BubbleShooter.LevelDesign.Scripts.LevelDatas.Converters
{
    public class BoardBottomDataConverter : JsonConverter<BoardBottomMapPosition>
    {
        public override BoardBottomMapPosition ReadJson(JsonReader reader, Type objectType, BoardBottomMapPosition existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if(reader.ReadInts(out int? x, out int? y))
            {
                reader.Read();
            }

            if(x.HasValue && y.HasValue)
            {
                return new BoardBottomMapPosition
                {
                    Position = new(x.Value, y.Value)
                };
            }

            return existingValue;
        }

        public override void WriteJson(JsonWriter writer, BoardBottomMapPosition value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            writer.WriteValue(value.Position.x);
            writer.WriteValue(value.Position.y);
            writer.WriteEndArray();
        }
    }
}
