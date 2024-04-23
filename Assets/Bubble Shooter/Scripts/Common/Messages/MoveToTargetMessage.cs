using Cysharp.Threading.Tasks;
using BubbleShooter.Scripts.Common.PlayDatas;

namespace BubbleShooter.Scripts.Common.Messages
{
    public struct MoveToTargetMessage
    {
        public UniTaskCompletionSource<MoveTargetData> Source;
    }
}
