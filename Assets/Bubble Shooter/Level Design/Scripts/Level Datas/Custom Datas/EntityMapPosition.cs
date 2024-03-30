using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.LevelDesign.Scripts.LevelDatas.Converters;
using BubbleShooter.Scripts.Gameplay.GameDatas;
using Newtonsoft.Json;

namespace BubbleShooter.LevelDesign.Scripts.LevelDatas.CustomDatas
{
    [JsonConverter(typeof(EntityMapPositionConverter))]
    public class EntityMapPosition : BaseMapPosition<EntityMapData>
    {

    }
}
