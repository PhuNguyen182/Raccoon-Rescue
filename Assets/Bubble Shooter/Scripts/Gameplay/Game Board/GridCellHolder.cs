using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter.Scripts.Gameplay.GameBoard
{
    public class GridCellHolder : MonoBehaviour
    {
        [SerializeField] private bool isCeil = false;
        
        public Vector3Int GridPosition { get; set; }

        public bool IsCeilGrid 
        { 
            get => isCeil; 
            set => isCeil = value; 
        }
    }
}
