using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.LevelDesign.Scripts.LevelDatas.Converters;
using Newtonsoft.Json;

namespace BubbleShooter.LevelDesign.Scripts.LevelDatas.CustomDatas
{
    [JsonConverter(typeof(CeilMapPositionConverter))]
    public class CeilMapPosition : BaseMapPosition<CeilMapData>
    {

    }

    public struct CeilMapData { }
}
