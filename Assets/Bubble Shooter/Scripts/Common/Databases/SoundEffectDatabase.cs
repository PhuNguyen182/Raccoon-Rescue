using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.PlayDatas;
using BubbleShooter.Scripts.Common.Enums;

namespace BubbleShooter.Scripts.Common.Databases
{
    [CreateAssetMenu(fileName = "Sound Effect Database", menuName = "Scriptable Objects/Databases/Sound Effect Database")]
    public class SoundEffectDatabase : ScriptableObject
    {
        [SerializeField] private List<SoundEffect> soundEffects;

        public SoundEffect GetSoundEffect(SoundEffectEnum effect)
        {
            return soundEffects.FirstOrDefault(e => e.EffectName == effect);
        }
    }
}
