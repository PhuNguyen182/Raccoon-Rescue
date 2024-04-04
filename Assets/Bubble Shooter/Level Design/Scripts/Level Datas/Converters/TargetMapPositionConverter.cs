using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using BubbleShooter.LevelDesign.Scripts.LevelDatas.CustomDatas;
using BubbleShooter.LevelDesign.Scripts.JsonUtils;
using BubbleShooter.Scripts.Common.Enums;

namespace BubbleShooter.LevelDesign.Scripts.LevelDatas.Converters
{
    public class TargetMapPositionConverter : JsonConverter<TargetMapPosition>
    {
        public override TargetMapPosition ReadJson(JsonReader reader, Type objectType, TargetMapPosition existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if(reader.ReadInts(out int? x, out int? y, out int? id, out int? color, out int? targetColor))
            {
                reader.Read();
            }

            if(x.HasValue && y.HasValue && id.HasValue && color.HasValue && targetColor.HasValue)
            {
                return new TargetMapPosition
                {
                    Position = new(x.Value, y.Value),
                    MapData = new TargetMapData
                    {
                        ID = id.Value,
                        Color = (EntityType)color.Value,
                        TargetColor = (TargetType)targetColor.Value
                    }
                };
            }

            return existingValue;
        }

        public override void WriteJson(JsonWriter writer, TargetMapPosition value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            writer.WriteValue(value.Position.x);
            writer.WriteValue(value.Position.y);
            writer.WriteValue(value.MapData.ID);
            writer.WriteValue((int)value.MapData.Color);
            writer.WriteValue((int)value.MapData.TargetColor);
            writer.WriteEndArray();
        }
    }
}
