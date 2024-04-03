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
    public class ColorMapDataConverter : JsonConverter<ColorMapData>
    {
        public override ColorMapData ReadJson(JsonReader reader, Type objectType, ColorMapData existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if(reader.ReadInts(out int? coefficient, out int? color))
            {
                reader.Read();
            }

            if(coefficient.HasValue && color.HasValue)
            {
                return new ColorMapData
                {
                    ColorProportion = new ColorProportion
                    {
                        Coefficient = coefficient.Value,
                        Color = (EntityType)color.Value
                    }
                };
            }

            return existingValue;
        }

        public override void WriteJson(JsonWriter writer, ColorMapData value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            writer.WriteValue(value.ColorProportion.Coefficient);
            writer.WriteValue((int)value.ColorProportion.Color);
            writer.WriteEndArray();
        }
    }
}
