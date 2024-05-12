using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Gameplay.Confiner;

namespace BubbleShooter.Scripts.Gameplay.Miscs
{
    public class GameDecorator : MonoBehaviour
    {
        [SerializeField] private Transform topCeil;
        [SerializeField] private Transform groundPointContainer;
        [SerializeField] private BoxCollider2D groundingArea;
        [SerializeField] private MainCharacter mainCharacter;

        [Header("Reflect Walls")]
        [SerializeField] private SideWall[] reflectWalls;

        public MainCharacter Character => mainCharacter;
        public Transform GroundPointContainer => groundPointContainer;

        public void SetTopCeilPosition(Vector3 position)
        {
            topCeil.position = position;
        }

        public Vector3 GetGroundingPoint()
        {
            float x = Random.Range(groundingArea.bounds.min.x, groundingArea.bounds.max.x);
            float y = Random.Range(groundingArea.bounds.min.y, groundingArea.bounds.max.y);
            return new(x, y);
        }

        public void ShowReflectWalls(bool active)
        {
            for (int i = 0; i < reflectWalls.Length; i++)
            {
                reflectWalls[i].ShowReflectWall(active);
            }
        }
    }
}
