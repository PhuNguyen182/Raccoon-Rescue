using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class LevelProgressData
{
    public int Level = 1;
    public List<LevelProgress> LevelProgresses;

    public void SetLevel(int level)
    {
        Level = level;
    }

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
            LevelProgress levelProgress = GetLevelProgress(progress.Level);
            
            if (progress.Star >= levelProgress.Star)
            {
                int index = LevelProgresses.FindIndex(p => p.Level == progress.Level);
                LevelProgresses[index] = progress;
            }
        }
        
        else
            LevelProgresses.Add(progress);
    }
}
