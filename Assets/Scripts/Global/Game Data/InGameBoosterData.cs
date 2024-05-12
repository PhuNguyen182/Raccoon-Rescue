using System;

[Serializable]
public class InGameBoosterData
{
    private int _colorfullCount;
    private int _targetAimCount;
    private int _randomBallCount;

    public InGameBoosterData(int colorfull, int targetAim, int randomBall)
    {
        _colorfullCount = colorfull;
        _targetAimCount = targetAim;
        _randomBallCount = randomBall;
    }

    public int ColorfulCount
    {
        get => _colorfullCount;
        set => _colorfullCount = value;
    }

    public int TargetAimCount
    {
        get => _targetAimCount;
        set => _targetAimCount = value;
    }

    public int RandomBallCount
    {
        get => _randomBallCount;
        set => _randomBallCount = value;
    }
}
