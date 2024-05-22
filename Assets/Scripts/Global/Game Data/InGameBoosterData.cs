using System;
using BubbleShooter.Scripts.Common.Enums;

[Serializable]
public class InGameBoosterData
{
    public int ColorfulCount;
    public int TargetAimCount;
    public int RandomBallCount;

    public InGameBoosterData(int colorfull, int targetAim, int randomBall)
    {
        ColorfulCount = colorfull;
        TargetAimCount = targetAim;
        RandomBallCount = randomBall;
    }

    public void AddBooster(IngameBoosterType boosterType, int amount)
    {
        switch (boosterType)
        {
            case IngameBoosterType.Colorful:
                ColorfulCount += amount;
                break;
            case IngameBoosterType.PreciseAimer:
                TargetAimCount += amount;
                break;
            case IngameBoosterType.ChangeBall:
                RandomBallCount += amount;
                break;
        }
    }
}
