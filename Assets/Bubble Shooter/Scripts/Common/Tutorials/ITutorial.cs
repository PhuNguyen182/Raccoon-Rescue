using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.Common.Tutorials
{
    public interface ITutorial
    {
        public UniTask Show();
        public UniTask Hide();
        public void DoNextStep();
    }
}
