using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter.Scripts.Gameplay.Miscs
{
    public class MainCharacter : MonoBehaviour
    {
        [SerializeField] private Animator characterAnimator;
        [SerializeField] private AudioSource characterAudio;

        [Header("Character Clips")]
        [SerializeField] private AudioClip shootClip;
        [SerializeField] private AudioClip[] characterClips;

        private static readonly int _shootHash = Animator.StringToHash("Shot");
        private static readonly int _loseHash = Animator.StringToHash("Lose");
        private static readonly int _continueHash = Animator.StringToHash("Continue");

        public void Shoot()
        {
            characterAudio.PlayOneShot(shootClip);
            characterAnimator.SetTrigger(_shootHash);
        }

        public void Cry()
        {
            characterAnimator.SetTrigger(_loseHash);
        }

        public void Continue()
        {
            int rand = Random.Range(0, 3);
            characterAudio.PlayOneShot(characterClips[rand]);
            characterAnimator.SetTrigger(_continueHash);
        }

        public void ResetPlayState()
        {
            characterAnimator.ResetTrigger(_continueHash);
        }

        public void ResetCryState()
        {
            characterAnimator.ResetTrigger(_loseHash);
        }
    }
}
