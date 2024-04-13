using System;
using BubbleShooter.LevelDesign.Scripts.LevelDatas.Converters;
using BubbleShooter.Scripts.Common.Enums;
using Newtonsoft.Json;

namespace BubbleShooter.LevelDesign.Scripts.LevelDatas.CustomDatas
{
    [JsonConverter(typeof(ColorMapDataConverter))]
    public class ColorMapData
    {
        public ColorProportion ColorProportion;
    }

    [Serializable]
    public struct ColorProportion
    {
        public int Coefficient;
        public EntityType Color;
    }

    public struct ColorDistribution
    {
        public float Probability;
        public EntityType Color;
    }
}
