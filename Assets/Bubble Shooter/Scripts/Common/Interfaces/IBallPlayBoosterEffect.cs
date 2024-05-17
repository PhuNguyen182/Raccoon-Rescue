using Cysharp.Threading.Tasks;
using BubbleShooter.Scripts.Common.Enums;

namespace BubbleShooter.Scripts.Common.Interfaces
{
    public interface IBallPlayBoosterEffect
    {
        public UniTask PlayBoosterEffect(EntityType booster);
        public void ReleaseEffect();
    }
}
