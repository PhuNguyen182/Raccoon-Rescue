using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Enums;

namespace BubbleShooter.LevelDesign.Scripts.CustomTiles
{
    [CreateAssetMenu(fileName = "Entity Tile", menuName = "Scriptable Objects/Level Builder/Custom Tiles/Entity Tile")]
    public class EntityTile : BaseEntityTile
    {
        public int HP;
    }
}
