using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Enums;

namespace BubbleShooter.LevelDesign.Scripts.CustomTiles
{
    [CreateAssetMenu(fileName = "Target Tile", menuName = "Scriptable Objects/Level Builder/Custom Tiles/Target Tile")]
    public class TargetTile : BaseEntityTile
    {
        public TargetType TargetType;
    }
}
