using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter.Scripts.Gameplay.GameEntities
{
    public class EntityGraphics : MonoBehaviour
    {
        [SerializeField] private Animator entityAnimator;
        [SerializeField] private SpriteRenderer entityRenderer;

        public void SetEntitySprite(Sprite sprite)
        {
            entityRenderer.sprite = sprite;
        }
    }
}
