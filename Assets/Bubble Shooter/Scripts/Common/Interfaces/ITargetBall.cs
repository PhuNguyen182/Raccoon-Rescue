using BubbleShooter.Scripts.Common.Enums;

namespace BubbleShooter.Scripts.Common.Interfaces
{
    public interface ITargetBall
    {
        public int ID { get; }
        public TargetType TargetColor { get; }

        public void SetID(int id);
        public void SetColor(EntityType color);
        public void SetTargetColor(TargetType targetColor);
        public void FreeTarget();
    }
}
