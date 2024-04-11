using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter.Scripts.Gameplay.GameEntities
{
    public class EntityGraphics : MonoBehaviour
    {
        [SerializeField] private Animator entityAnimator;
        [SerializeField] private SpriteRenderer entityRenderer;
        [SerializeField] private Renderer[] additionalRenderers;

        public Animator EntityAnimator => entityAnimator;

        public void SetEntitySprite(Sprite sprite)
        {
            entityRenderer.sprite = sprite;
        }

        public void ChangeLayer(string layerName)
        {
            int layerID = SortingLayer.NameToID(layerName);
            entityRenderer.sortingLayerID = layerID;

            for (int i = 0; i < additionalRenderers.Length; i++)
            {
                if (additionalRenderers[i] != null)
                    additionalRenderers[i].sortingLayerID = layerID;
            }
        }
    }
}
