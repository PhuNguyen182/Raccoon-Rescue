using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RaccoonRescue.Scripts.Gameplay.GameEntities
{
    public abstract class BaseEntities : MonoBehaviour
    {
        [SerializeField] protected SpriteRenderer entityRenderer;
        [SerializeField] protected Animator entityAnimator;
    }
}
