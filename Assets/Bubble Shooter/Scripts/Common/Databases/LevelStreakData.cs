using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter.Scripts.Common.Databases
{
    [CreateAssetMenu(fileName = "Level Download Streaks", menuName = "Scriptable Objects/Databases/Level Download Streaks")]
    public class LevelStreakData : ScriptableObject
    {
        [SerializeField] private List<Range<int>> levelRanges;

        public List<Range<int>> LevelRanges => levelRanges;
    }
}
