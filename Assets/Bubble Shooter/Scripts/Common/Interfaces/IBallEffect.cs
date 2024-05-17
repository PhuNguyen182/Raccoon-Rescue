namespace BubbleShooter.Scripts.Common.Interfaces
{
    public interface IBallEffect
    {
        public void ToggleEffect(bool active);
        public void PlayBlastEffect(bool isFallen);
        public void PlayColorfulEffect();
    }
}
