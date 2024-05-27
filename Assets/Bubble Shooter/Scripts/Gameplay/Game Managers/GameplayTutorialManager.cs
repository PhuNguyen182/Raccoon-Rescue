using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Tutorials;

namespace BubbleShooter.Scripts.Gameplay.GameManagers
{
    public class GameplayTutorialManager : MonoBehaviour
    {
        [SerializeField] private TutorialLevel1 tutorialLevel1;

        private Dictionary<int, BaseTutorial> _tutorials;

        private void Awake()
        {
            _tutorials = new()
            {
                {1, tutorialLevel1 }
            };
        }

        public BaseTutorial GetTutorial(int level)
        {
            return _tutorials.TryGetValue(level, out BaseTutorial tutorial) ? tutorial : null;
        }
    }
}
