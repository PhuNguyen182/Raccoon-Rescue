using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using BubbleShooter.Scripts.Common.Enums;
using BubbleShooter.LevelDesign.Scripts.LevelDatas.Converters;

namespace BubbleShooter.LevelDesign.Scripts.LevelDatas.CustomDatas
{
    [JsonConverter(typeof(TargetMapPositionConverter))]
    public class TargetMapPosition : BaseMapPosition<TargetMapData>
    {

    }

    public struct TargetMapData
    {
        public int ID;
        public EntityType Color;
        public TargetType TargetColor;
    }
}
