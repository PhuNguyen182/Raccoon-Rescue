using System;
using System.Collections.Generic;

[Serializable]
public class LevelProgressData
{
    public List<LevelProgress> LevelProgresses { get; }

    public LevelProgressData(List<LevelProgress> levelProgresses)
    {
        if (levelProgresses == null || levelProgresses.Count <= 0)
            LevelProgresses = new();

        LevelProgresses = levelProgresses;
    }

    public void Append(LevelProgress progress)
    {
        if (LevelProgresses.Contains(progress))
        {
            int index = LevelProgresses.IndexOf(progress);
            LevelProgresses[index] = progress;
        }
        
        else
            LevelProgresses.Add(progress);
    }
}
