using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Enums;
using Sirenix.OdinInspector;

namespace BubbleShooter.Scripts.Gameplay.Miscs
{
    public class DummyBall : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer ballPreview;

        [Header("Ball Colors")]
        [FoldoutGroup("Ball Colors")]
        [SerializeField] private Sprite blue;
        [FoldoutGroup("Ball Colors")]
        [SerializeField] private Sprite green;
        [FoldoutGroup("Ball Colors")]
        [SerializeField] private Sprite orange;
        [FoldoutGroup("Ball Colors")]
        [SerializeField] private Sprite red;
        [FoldoutGroup("Ball Colors")]
        [SerializeField] private Sprite violet;
        [FoldoutGroup("Ball Colors")]
        [SerializeField] private Sprite yellow;
        [FoldoutGroup("Ball Colors")]
        [SerializeField] private Sprite colorful;

        public void SetBallColor(bool isActive, EntityType ballColor)
        {
            Sprite color = ballColor switch
            {
                EntityType.Blue => blue,
                EntityType.Green => green,
                EntityType.Orange => orange,
                EntityType.Red => red,
                EntityType.Violet => violet,
                EntityType.Yellow => yellow,
                EntityType.ColorfulBall => colorful,
                _ => null
            };

            ballPreview.sprite = color;
            ballPreview.gameObject.SetActive(isActive);
        }
    }
}
