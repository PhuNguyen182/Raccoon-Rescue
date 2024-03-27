using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter.Scripts.Gameplay.GameBoard
{
    public class GridCellHolder : MonoBehaviour
    {
        [SerializeField] private Collider2D cellCollider;

        public bool IsCeilGrid { get; set; }
        public Vector3Int GridPosition { get; set; }

        public void SetColliderEnable(bool enable)
        {
            cellCollider.enabled = enable;
        }
    }
}
