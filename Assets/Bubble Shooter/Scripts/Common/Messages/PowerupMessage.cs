using BubbleShooter.Scripts.Common.Enums;

namespace BubbleShooter.Scripts.Common.Messages
{
    public struct PowerupMessage
    {
        public int Amount;
        public EntityType PowerupColor;
        public ReactiveValueCommand Command;
    }
}
