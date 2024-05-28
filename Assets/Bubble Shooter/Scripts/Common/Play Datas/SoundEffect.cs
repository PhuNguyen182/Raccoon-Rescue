using System;
using UnityEngine;
using BubbleShooter.Scripts.Common.Enums;

namespace BubbleShooter.Scripts.Common.PlayDatas
{
    [Serializable]
    public class SoundEffect
    {
        public AudioClip EffectClip;
        public SoundEffectEnum EffectName;
    }
}
