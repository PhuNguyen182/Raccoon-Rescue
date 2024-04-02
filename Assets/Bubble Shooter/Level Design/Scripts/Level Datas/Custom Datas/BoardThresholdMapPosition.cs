using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using BubbleShooter.LevelDesign.Scripts.LevelDatas.Converters;

namespace BubbleShooter.LevelDesign.Scripts.LevelDatas.CustomDatas
{
    [JsonConverter(typeof(BoardThresholdMapPositionConverter))]
    public class BoardThresholdMapPosition : BaseMapPosition<BoardThresholdData>
    {

    }

    public struct BoardThresholdData { }
}
