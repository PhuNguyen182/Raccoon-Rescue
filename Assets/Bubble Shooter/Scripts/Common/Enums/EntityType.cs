using System;

namespace BubbleShooter.Scripts.Common.Enums
{
    [Serializable]
    public enum EntityType
    {
        None = 0, // Initialize value

        #region Basic color bubbles
        Red = 1,
        Yellow = 2,
        Green = 3,
        Blue = 4,
        Violet = 5,
        Orange = 6,
        Random = 7, // Random means pick one of basic color bubble randomly
        #endregion

        #region Boosters
        FireBall = 8,
        LeafBall = 9,
        IceBall = 10,
        WaterBall = 11,
        SunBall = 12,
        #endregion

        #region Special bubbles
        UnbreakableBall = 13,
        WoodenBall = 14,
        ColorfulBall = 15,
        #endregion
    }
}
