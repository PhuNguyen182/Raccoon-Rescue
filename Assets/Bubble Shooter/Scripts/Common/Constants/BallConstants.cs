namespace BubbleShooter.Scripts.Common.Constants
{
    public static class BallConstants
    {
        public const float BallMoveSpeed = 24f;
        public const float CheckGridOffset = 0.35f;
        public const float GridSnapDistance = 0.7f;
        public const float SpreadShootAngle = 30f;
        
        public const float MinForce = 8f;
        public const float MaxForce = 10f;
        public const float WinForce = 18f;

        public const string CeilLayerName = "Ceil";
        public const string BallLayerName = "Ball";
        public const string GridLayerName = "Grid";
        public const string NormalLayer = "Objects";
        public const string HigherLayer = "HigherObjects";
        public const string ReflectLayerName = "ReflectLine";
        public const string DefaultLayer = "Default";
        public const string BallLayer = "Ball";
    }
}
