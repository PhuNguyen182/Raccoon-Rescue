using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter.Scripts.Effects.BallEffects
{
    [RequireComponent(typeof(AutoDespawn))]
    [RequireComponent(typeof(AudioSource))]
    public class BallSoundEffect : MonoBehaviour
    {
        [SerializeField] private AudioSource soundPlayer;
        [SerializeField] private AutoDespawn autoDespawn;

        public void PlaySound(AudioClip clip)
        {
            autoDespawn.SetDuration(clip.length);
            soundPlayer.PlayOneShot(clip);
        }

        private void OnDisable()
        {
            autoDespawn.SetDuration(1);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            autoDespawn ??= GetComponent<AutoDespawn>();
            soundPlayer ??= GetComponent<AudioSource>();
        }
#endif
    }
}
