using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class LevelProgressData
{
    public int Level = 1;
    public List<LevelProgress> LevelProgresses;

    public LevelProgressData(List<LevelProgress> levelProgresses)
    {
        if (levelProgresses == null || levelProgresses.Count <= 0)
            LevelProgresses = new();

        LevelProgresses = levelProgresses;
    }

    public LevelProgress GetLevelProgress(int level)
    {
        return LevelProgresses.FirstOrDefault(x => x.Level == level);
    }

    public bool IsLevelComplete(int level)
    {
        return LevelProgresses.Exists(x => x.Level == level);
    }

    public void Append(LevelProgress progress)
    {
        if (LevelProgresses.Exists(p => p.Level == progress.Level))
        {
            int index = LevelProgresses.IndexOf(progress);
            LevelProgresses[index] = progress;
        }
        
        else
            LevelProgresses.Add(progress);
    }
}
