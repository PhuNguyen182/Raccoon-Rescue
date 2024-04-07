using System;

namespace BubbleShooter.Scripts.Common.Enums
{
    [Serializable]
    public enum BallMovementState
    {
        None = 0,
        Ready = 1,
        Fixed = 2,
        Moving = 3,
        Fall = 4,
    }
}
