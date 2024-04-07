using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter.Scripts.Gameplay.Miscs
{
    public class MainCharacter : MonoBehaviour
    {
        [SerializeField] private Animator characterAnimator;

        private static readonly int _shootHash = Animator.StringToHash("Shot");
        private static readonly int _loseHash = Animator.StringToHash("Lose");
        private static readonly int _continueHash = Animator.StringToHash("Continue");

        public void Shoot()
        {
            characterAnimator.SetTrigger(_shootHash);
        }

        public void Cry()
        {
            characterAnimator.SetTrigger(_loseHash);
        }

        public void Continue()
        {
            characterAnimator.SetTrigger(_continueHash);
        }
    }
}
