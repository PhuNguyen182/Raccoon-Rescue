namespace BubbleShooter.Scripts.Common.Interfaces
{
    public interface IBreakable
    {
        public bool EasyBreak { get; }
        public bool Break();
    }
}
