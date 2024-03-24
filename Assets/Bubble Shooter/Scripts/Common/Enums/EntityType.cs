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
        FireBubble = 8,
        LeafBubble = 9,
        IceBubble = 10,
        WaterBubble = 11,
        #endregion

        #region Special bubbles
        UnbreakableBubble = 12,
        WoodenBubble = 13,
        #endregion
    }
}
