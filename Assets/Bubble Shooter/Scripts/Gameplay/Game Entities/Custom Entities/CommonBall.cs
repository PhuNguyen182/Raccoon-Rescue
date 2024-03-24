using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter.Scripts.Gameplay.GameEntities.CustomEntities
{
    public class CommonBall : BaseBall
    {
        public BallMovement BallMovement;

        public override void Blast()
        {
            
        }

        public override void Clear()
        {
            SimplePool.Despawn(this.gameObject);
        }
    }
}
