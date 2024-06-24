using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.LevelDesign.Scripts.LevelDatas.Converters;
using Newtonsoft.Json;

namespace BubbleShooter.LevelDesign.Scripts.LevelDatas.CustomDatas
{
    [JsonConverter(typeof(BoardBottomDataConverter))]
    public class BoardBottomMapPosition : BaseMapPosition<BoardBottomData>
    {

    }

    public struct BoardBottomData { }
}
