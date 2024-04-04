using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter.Scripts.Gameplay.GameEntities
{
    public class EntityGraphics : MonoBehaviour
    {
        [SerializeField] private Animator entityAnimator;
        [SerializeField] private SpriteRenderer entityRenderer;

        private static readonly int _sadEmotionHash = Animator.StringToHash("SadEmotion");

        public void SetEntitySprite(Sprite sprite)
        {
            entityRenderer.sprite = sprite;
        }

        public void PlaySadEmotion()
        {
            if (entityAnimator != null)
            {
                int rand = Random.Range(1, 7);
                entityAnimator.SetInteger(_sadEmotionHash, rand);
            }
        }
    }
}
