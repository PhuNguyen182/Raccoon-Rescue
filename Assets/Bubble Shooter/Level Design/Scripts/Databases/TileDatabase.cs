using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.LevelDesign.Scripts.CustomTiles;
using BubbleShooter.LevelDesign.Scripts.BoardTiles;
using BubbleShooter.Scripts.Common.Enums;
using System.Linq;

namespace BubbleShooter.LevelDesign.Scripts.Databases
{
    [CreateAssetMenu(fileName = "Tile Database", menuName = "Scriptable Objects/Level Builder/Databases/Tile Database")]
    public class TileDatabase : ScriptableObject
    {
        [SerializeField] private BoardTile boardTile;
        [SerializeField] private CeilTile ceilTile;
        [SerializeField] private BoardBottomTile boardBottomTile;
        [SerializeField] private List<BallTile> ballTiles;
        [SerializeField] private List<EntityTile> entityTiles;
        [SerializeField] private List<TargetTile> targetTiles;

        public BoardBottomTile GetBoardBottomTile() => boardBottomTile;

        public BoardTile GetBoardTile() => boardTile;

        public CeilTile GetCeilTile() => ceilTile;

        public BallTile FindBallTile(int id, EntityType entityType)
        {
            return ballTiles.FirstOrDefault(tile => tile.ID == id && tile.EntityType == entityType);
        }

        public EntityTile FindEntityTile(int id, int hp, EntityType entityType)
        {
            return entityTiles.FirstOrDefault(tile => tile.ID == id && tile.HP == hp && tile.EntityType == entityType);
        }

        public TargetTile FindTargetTile(int id, EntityType color, TargetType targetColor)
        {
            return targetTiles.FirstOrDefault(tile => tile.ID == id && tile.EntityType == color && tile.TargetType == targetColor);
        }
    }
}
