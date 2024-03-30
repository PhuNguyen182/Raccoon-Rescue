using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.LevelDesign.Scripts.LevelDatas.CustomDatas;
using BubbleShooter.LevelDesign.Scripts.JsonUtils;
using BubbleShooter.Scripts.Gameplay.GameDatas;
using BubbleShooter.Scripts.Common.Enums;
using Newtonsoft.Json;

namespace BubbleShooter.LevelDesign.Scripts.LevelDatas.Converters
{
    public class EntityMapPositionConverter : JsonConverter<EntityMapPosition>
    {
        public override EntityMapPosition ReadJson(JsonReader reader, Type objectType, EntityMapPosition existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if(reader.ReadInts(out int? x, out int? y, out int? id, out int? hp, out int? entityType))
            {
                reader.Read();
            }

            if(x.HasValue && y.HasValue && id.HasValue && hp.HasValue && entityType.HasValue)
            {
                return new EntityMapPosition
                {
                    Position = new Vector3Int(x.Value, y.Value),
                    MapData = new EntityMapData
                    {
                        ID = id.Value,
                        HP = hp.Value,
                        EntityType = (EntityType)entityType.Value
                    }
                };
            }

            return existingValue;
        }

        public override void WriteJson(JsonWriter writer, EntityMapPosition value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            writer.WriteValue(value.Position.x);
            writer.WriteValue(value.Position.y);
            writer.WriteValue(value.MapData.ID);
            writer.WriteValue(value.MapData.HP);
            writer.WriteValue((int)value.MapData.EntityType);
            writer.WriteEndArray();
        }
    }
}
