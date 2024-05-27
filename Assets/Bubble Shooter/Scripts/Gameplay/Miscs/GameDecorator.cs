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
        [SerializeField] private SpriteRenderer background;

        [Header("Reflect Walls")]
        [SerializeField] private SideWall[] reflectWalls;

        [Header("Backgrounds")]
        [SerializeField] private Sprite[] backgrounds;

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

        public void SetBackground(int level)
        {
            int backgroundIndex = level / 20;
            backgroundIndex = Mathf.Clamp(backgroundIndex, 0, 5);
            background.sprite = backgrounds[backgroundIndex];
        }
    }
}
