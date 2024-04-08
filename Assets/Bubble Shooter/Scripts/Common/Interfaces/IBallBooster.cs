using Cysharp.Threading.Tasks;
using BubbleShooter.Scripts.Common.Enums;

namespace BubbleShooter.Scripts.Common.Interfaces
{
    public interface IBallBooster
    {
        public bool IsIgnored { get; set; }
        public UniTask Activate();
        public UniTask Explode();
    }
}
