using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class LevelData
{
    [SerializeField] private List<LevelProgress> _progress;

    public int GetLevel(StatId id)
    {
        foreach (var levelProgress in _progress)
        {
            if (levelProgress.Id == id)
            {
                return levelProgress.Level;
            }
        }
        return 0;
        //var progress = _progress.FirstOrDefault(x => x.Id == id);
        //return progress?.Level ?? 0;
    }
    public void LevelUp(StatId id)
    {
        var progess = _progress.FirstOrDefault(_ => _.Id == id);
        if (progess == null)
        {
            _progress.Add(new LevelProgress(id, 1));
        }
        else
        {
            progess.Level++;
        }
    }
}
[Serializable]
public class LevelProgress
{
    public StatId Id;
    public int Level;

    public LevelProgress(StatId id, int level)
    {
        Id = id;
        Level = level;
    }
}