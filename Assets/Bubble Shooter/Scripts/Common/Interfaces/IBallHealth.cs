namespace BubbleShooter.Scripts.Common.Interfaces
{
    public interface IBallHealth
    {
        public int MaxHP { get; }

        public void SetMaxHP(int maxHP);
    }
}
