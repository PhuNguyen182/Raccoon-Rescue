using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Enums;

namespace BubbleShooter.Scripts.Gameplay.Models
{
    public class TargetModel : MonoBehaviour
    {
        public EntityType EntityType;
        public TargetType TargetType;
        public SpriteRenderer BallSprite;
    }
}
