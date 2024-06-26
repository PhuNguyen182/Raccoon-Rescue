using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter.Scripts.Gameplay.GameEntities
{
    public class EntityAudio : MonoBehaviour
    {
        [SerializeField] private AudioSource soundPlayer;

        public void PlaySoundOneShot(AudioClip audioClip, float volumeScale = 1)
        {
            if(soundPlayer != null && audioClip != null)
            {
                soundPlayer.PlayOneShot(audioClip, volumeScale);
            }
        }

        public void PlaySound(AudioClip audioClip, float volumeScale = 1)
        {
            if (soundPlayer != null && audioClip != null)
            {
                soundPlayer.clip = audioClip;
                soundPlayer.volume = volumeScale;
                soundPlayer.Play();
            }
        }
    }
}
