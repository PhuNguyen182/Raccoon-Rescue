using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Gameplay.GameEntities;
using BubbleShooter.Scripts.Gameplay.GameEntities.CustomBalls;
using BubbleShooter.Scripts.Gameplay.Models;

namespace BubbleShooter.Scripts.Common.Databases
{
    [CreateAssetMenu(fileName = "Entity Database", menuName = "Scriptable Objects/Databases/Entity Database")]
    public class EntityDatabase : ScriptableObject
    {
        public CommonBall BallPrefab;
        public CommonBallModel[] BallModels;
        public BaseEntity[] Boosters;
        public BaseEntity[] BallEntities;
    }
}
